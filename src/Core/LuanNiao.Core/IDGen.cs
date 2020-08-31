using System;
using System.Collections.Generic;
using System.Text;

namespace LuanNiao.Core
{
    /// <summary>
    /// this IDGen use twitter's  algorithm ---> snowflake like
    /// </summary>
    public class IDGen
    {
        private static IDGen _iDWorker = null;
        public const long Twepoch = 1288834974657L;

        private const int _workerIdBits = 5;

        private const int _datacenterIdBits = 5;
        private const int _sequenceBits = 12;

        private const long _maxWorkerId = -1L ^ (-1L << _workerIdBits);

        private const long _maxDatacenterId = -1L ^ (-1L << _datacenterIdBits);

        private const int _workerIdShift = _sequenceBits;
        private const int _datacenterIdShift = _sequenceBits + _workerIdBits;

        public const int TimestampLeftShift = _sequenceBits + _workerIdBits + _datacenterIdBits;

        private const long _sequenceMask = -1L ^ (-1L << _sequenceBits);

        private long _sequence = 0L;
        private long _lastTimestamp = -1L;

        private IDGen() : this(0, 0)
        {
        }
        private IDGen(long workerId, long datacenterId, long sequence = 0L)
        {
            WorkerId = workerId;
            DatacenterId = datacenterId;
            _sequence = sequence;
            if (workerId > _maxWorkerId || workerId < 0)
            {
                throw new ArgumentException(String.Format("worker Id can't be greater than {0} or less than 0", _maxWorkerId));
            }

            if (datacenterId > _maxDatacenterId || datacenterId < 0)
            {
                throw new ArgumentException(String.Format("datacenter Id can't be greater than {0} or less than 0", _maxDatacenterId));
            }
        }

        public static IDGen GetInstance(long workerID = 12L, long datacenter = 12L)
        {
            if (_iDWorker == null)
            {
                _iDWorker = new IDGen(workerID, datacenter);
            }
            return _iDWorker;
        }

        public long WorkerId { get; protected set; }
        public long DatacenterId { get; protected set; }

        public long Sequence
        {
            get { return _sequence; }
            internal set { _sequence = value; }
        }


        private readonly object _lock = new Object();
        public virtual long NextId()
        {
            lock (_lock)
            { 
                var timestamp = TimeGen();

                if (timestamp < _lastTimestamp)
                {
                    throw new Exception(String.Format(
                        "Clock moved backwards.  Refusing to generate id for {0} milliseconds", _lastTimestamp - timestamp));
                }

                if (_lastTimestamp == timestamp)
                {
                    _sequence = (_sequence + 1) & _sequenceMask;
                    if (_sequence == 0)
                    {
                        timestamp = TilNextMillis(_lastTimestamp);
                    }
                }
                else
                {
                    _sequence = 0;
                }

                _lastTimestamp = timestamp;
                var id = ((timestamp - Twepoch) << TimestampLeftShift) |
                         (DatacenterId << _datacenterIdShift) |
                         (WorkerId << _workerIdShift) | _sequence;

                return id;
            }
        }

        protected virtual long TilNextMillis(long lastTimestamp)
        {
            var timestamp = TimeGen();
            while (timestamp <= lastTimestamp)
            {
                timestamp = TimeGen();
            }
            return timestamp;
        }

        protected virtual long TimeGen()
        {
            return IdWorkSystem.CurrentTimeMillis();
        }
    }
    internal static class IdWorkSystem
    {
        public static Func<long> currentTimeFunc = InternalCurrentTimeMillis;

        public static long CurrentTimeMillis()
        {
            return currentTimeFunc();
        }

        public static IDisposable StubCurrentTime(Func<long> func)
        {
            currentTimeFunc = func;
            return new DisposableAction(() =>
            {
                currentTimeFunc = InternalCurrentTimeMillis;
            });
        }

        public static IDisposable StubCurrentTime(long millis)
        {
            currentTimeFunc = () => millis;
            return new DisposableAction(() =>
            {
                currentTimeFunc = InternalCurrentTimeMillis;
            });
        }

        private static readonly DateTime _jan1st1970 = new DateTime
           (1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private static long InternalCurrentTimeMillis()
        {
            return (long)(DateTime.UtcNow - _jan1st1970).TotalMilliseconds;
        }
    }
    public class DisposableAction : IDisposable
    {
        private readonly Action _action;

        public DisposableAction(Action action)
        {
            if (action == null)
                throw new ArgumentNullException("action");
            _action = action;
        }

        public void Dispose()
        {
            _action();
        }
    }
}
