using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using LuanNiao.Yggdrasil.Logger.AbsInfos;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;


namespace LuanNiao.Yggdrasil.MQ.RabbitMQ
{
    /// <summary>
    /// RabbitMQ管理器
    /// </summary>
    public class RabbitMQManager : AbsMQManager
    {
        /// <summary>
        /// 工厂池
        /// </summary>
        private readonly ConcurrentDictionary<string, ConnectionFactory> _factoryPool = new();

        /// <summary>
        /// 链接池
        /// </summary>
        private readonly ConcurrentDictionary<string, IConnection> _connectionPool = new();

        /// <summary>
        /// 通道池
        /// </summary>
        private readonly ConcurrentDictionary<string, IModel> _channelPool = new();

        public RabbitMQManager(AbsLNLoggerProvider logger) : base(logger.CreateLogger<RabbitMQManager>())
        {
        }


        /// <summary>
        /// 连接至某个服务
        /// </summary>
        private IConnection GetConnect(string host, string userName, string password, string virtualHostName)
        {
            var factory = _factoryPool.GetOrAdd(host, key =>
            {
                Uri uri = new(host);
                ConnectionFactory connectionFactory = new()
                {
                    UserName = userName,
                    Password = password,
                    VirtualHost = virtualHostName,
                    Endpoint = new AmqpTcpEndpoint(uri),
                    AutomaticRecoveryEnabled = true
                };
                return connectionFactory;
            });

            var connection = _connectionPool.GetOrAdd(host, key =>
            {
                return factory.CreateConnection();
            });

            return connection;
        }

        /// <summary>
        /// 給本地的MQ服务发送一条消息
        /// </summary>
        /// <param name="host">主机地址,只支持:amqp</param>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="queueName">队列名</param>
        /// <param name="virtualHost">虚拟主机名</param>
        /// <param name="message">消息内容</param>  
        /// <param name="exchangeName">特定的交换机名称,不填默认amq.direct,请IT人员配置好</param>
        /// <param name="routingKey">特定的交换机路径名,不填默认就是MQ的名字.请IT人员配置好</param>  
        /// <returns>是否推送成功</returns>
        public bool SendMessageTo(string host, string userName, string password, string virtualHost, string queueName, string message, string? exchangeName = null, string? routingKey = null)
        {
            try
            {
                var conn = GetConnect(host, userName, password, virtualHost);
                var channel = _channelPool.GetOrAdd($"{host}_SendMeessageToChannel", key =>
                {
                    var channel = conn.CreateModel();
                    return channel;
                });
                var props = channel.CreateBasicProperties();
                props.DeliveryMode = 2;
                channel.BasicPublish(exchangeName ?? "amq.direct", routingKey ?? queueName, false, props, Encoding.UTF8.GetBytes(message));
            }
            catch (Exception ex)
            {
                Logger.Error($"Sens message to server failed.", exception: ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 关闭一个通道
        /// </summary>
        /// <param name="channelToken">通道key</param>
        public void CloseConnection(string channelToken)
        {
            if (_channelPool.TryRemove(channelToken, out var channel))
            {
                channel.Close();
                channel.Dispose();
            }
        }

        /// <summary>
        /// 从本地的MQ中获取消息,连续性
        /// </summary>
        /// <param name="host">主机地址,只支持:amqp</param>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="queueName">队列名</param>
        /// <param name="virtualHost">虚拟主机名</param>
        /// <param name="callBackMethod">回调函数,注意这是持续性的,考虑好释放时间,函数返回true则删除服务器消息,返回false则认为消费失败,重新回队列</param>       
        /// <param name="canMutileThread">是否支持多线程,默认是true,如果填写false,那么将会进行串行回调</param>   
        /// <param name="exclusive">是否排他,也就是系统中不要出现第二个消费器</param>           
        public string? GotMessageFrom(string host, string userName, string password, string virtualHost, string queueName, Func<string, bool> callBackMethod, bool canMutileThread = true, bool exclusive = false)
        {
            var channelToken = Guid.NewGuid().ToString("N");
            var conn = GetConnect(host, userName, password, virtualHost);
            var channel = conn.CreateModel();
            try
            {
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (sender, e) =>
                {
                    var message = Encoding.UTF8.GetString(e.Body.Span);

                    if (canMutileThread)
                    {
                        _ = Task.Run(() =>
                        {
                            CallbackInvoker(callBackMethod, channel, e.DeliveryTag, message);
                        });
                    }
                    else
                    {
                        CallbackInvoker(callBackMethod, channel, e.DeliveryTag, message);
                    }
                };
                _ = channel.BasicConsume(queueName, false, channelToken, true, exclusive, null, consumer);
                if (!_channelPool.TryAdd(channelToken, channel))
                {
                    throw new Exception("Try add to pool failed.");
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Get message from server failed.", exception: ex);
                channel.Close();
                channel.Dispose();
                return null;
            }
            return channelToken;
        }

        /// <summary>
        /// 从本地的MQ中获取消息,只触发一次
        /// </summary>        
        /// <param name="host">主机地址,只支持:amqp</param>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="queueName">队列名</param>
        /// <param name="virtualHost">虚拟主机名</param>
        /// <param name="callBackMethod">回调函数,注意这是持续性的,考虑好释放时间,函数返回true则删除服务器消息,返回false则认为消费失败,重新回队列</param>        
        /// <param name="exclusive">是否排他,也就是系统中不要出现第二个消费器</param>    
        public void GotOneMessageFrom(string host, string userName, string password, string virtualHost, string queueName, Func<string, bool> callBackMethod, bool exclusive = false)
        {
            var channelToken = Guid.NewGuid().ToString("N");
            var conn = GetConnect(host, userName, password, virtualHost);
            var channel = conn.CreateModel();
            channel.BasicQos(0, 1, false);
            var useTimes = 0;
            try
            {
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (sender, e) =>
                {
                    if (Interlocked.Increment(ref useTimes) > 1)
                    {
                        return;
                    }
                    var message = Encoding.UTF8.GetString(e.Body.Span);
                    CallbackInvoker(callBackMethod, channel, e.DeliveryTag, message);
                    channel.Close();
                    channel.Dispose();
                };
                _ = channel.BasicConsume(queueName, false, channelToken, true, exclusive, null, consumer);
            }
            catch (Exception ex)
            {
                Logger.Error($"Get message from server failed.", exception: ex);
            }
        }

        /// <summary>
        /// 调用具体回调函数
        /// </summary>
        /// <param name="callBackMethod">回调函数</param>
        /// <param name="channel">通道</param>
        /// <param name="tag">tag标</param>
        /// <param name="message">消息体</param>
        private void CallbackInvoker(Func<string, bool> callBackMethod, IModel channel, ulong tag, string message)
        {
            try
            {
                var es = callBackMethod?.Invoke(message);
                channel.BasicNack(tag, false, es == null || !es.Value);
            }
            catch (Exception ex)
            {
                Logger.Error($"Invoke call Back got  failed.", exception: ex);
                channel.BasicNack(tag, false, true);
            }
        }

        /// <summary>
        /// 从本地的MQ中获取消息,连续性
        /// </summary>
        /// <param name="host">主机地址,只支持:amqp</param>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="queueName">队列名</param>
        /// <param name="virtualHost">虚拟主机名</param>
        /// <param name="callBackMethod">回调函数,注意这是持续性的,考虑好释放时间,函数返回true则删除服务器消息,返回false则认为消费失败,重新回队列</param>       
        /// <param name="canMutileThread">是否支持多线程,默认是true,如果填写false,那么将会进行串行回调</param>   
        /// <param name="exclusive">是否排他,也就是系统中不要出现第二个消费器</param>           
        public string? GotMessageFromAsync(string host, string userName, string password, string virtualHost, string queueName, Func<string, Task<bool>> callBackMethod, bool canMutileThread = true, bool exclusive = false)
        {
            var channelToken = Guid.NewGuid().ToString("N");
            var conn = GetConnect(host, userName, password, virtualHost);
            var channel = conn.CreateModel();
            try
            {
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += async (sender, e) =>
                {
                    var message = Encoding.UTF8.GetString(e.Body.Span);

                    if (canMutileThread)
                    {
                        _ = Task.Run(async () =>
                        {
                            await CallbackInvokerAsync(callBackMethod, channel, e.DeliveryTag, message);
                        });
                    }
                    else
                    {
                        await CallbackInvokerAsync(callBackMethod, channel, e.DeliveryTag, message);
                    }
                };
                _ = channel.BasicConsume(queueName, false, channelToken, true, exclusive, null, consumer);
                if (!_channelPool.TryAdd(channelToken, channel))
                {
                    throw new Exception("Try add to pool failed.");
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Get message from server failed.", exception: ex);
                channel.Close();
                channel.Dispose();
                return null;
            }
            return channelToken;
        }

        /// <summary>
        /// 调用具体回调函数
        /// </summary>
        /// <param name="callBackMethod">回调函数</param>
        /// <param name="channel">通道</param>
        /// <param name="tag">tag标</param>
        /// <param name="message">消息体</param>
        private async Task CallbackInvokerAsync(Func<string, Task<bool>> callBackMethod, IModel channel, ulong tag, string message)
        {
            if (callBackMethod is null)
            {
                return;
            }
            try
            {
                var es = await callBackMethod.Invoke(message);
                channel.BasicNack(tag, false, !es);
            }
            catch (Exception ex)
            {
                Logger.Error($"Invoke call Back got  failed.", exception: ex);
                channel.BasicNack(tag, false, true);
            }
        }
    }
}
