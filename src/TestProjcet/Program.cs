using LuanNiao.Yggdrasil;
using LuanNiao.Yggdrasil.Logger;
using LuanNiao.Yggdrasil.MQ.RabbitMQ;

var p = LNLoggerProvider.GetInstance(new LuanNiao.Yggdrasil.Logger.Models.LogOptions() { });

new AbsYggdrasilContext(
    new LuanNiao.Yggdrasil.Cache.CacheManager(p), "zxc"
    , p
    , new LuanNiao.Yggdrasil.DB.DataBaseManager(p, "", 11, new string[] { })
    , new LuanNiao.Yggdrasil.PluginLoader.PluginLoaderManager(p)
    , new RabbitMQManager(p)
    , new LuanNiao.Yggdrasil.Auth.AuthManager("", "", "", "", "")
    , ""
    );