using BenchmarkDotNet.Attributes;
using LuanNiao.Core.SourcePool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z.LuanNiao.Core.Performance.Test
{
    /*
 Method	        Mean	    Error	    StdDev	    Gen 0	    Gen 1	Gen 2	Allocated
 PushItem	    214.0 ns	4.11 ns	    3.64 ns	    0.0076	    0.0026	-	    48 B
 DeQUEUE	        366.2 ns	2.84 ns	    2.66 ns	    0.0114	    -	    -	    48 B         
  */
    /// <summary>
    /// MQ Performance test
    /// </summary>
    [MemoryDiagnoser]
    public class SourceQueueTest
    {
        [Benchmark]
        public void PushItem()
        {
            _ = ResourceQueue.Instance().PushItem(new DefaultResourceItem());
        }

        [Benchmark]
        public void DeQUEUE()
        {
            _ = ResourceQueue.Instance().PushItem(new DefaultResourceItem());
            ResourceQueue.Instance().Fetch<DefaultResourceItem>(item =>
            {

                return true;
            }).Wait();
        }

        /*
            Method                  Mean                    Error           StdDev          Gen 0       Gen 1       Gen 2   Allocated   
            MutipleMessage_100_000  38,312,240.1 ns/38MS    495,222.56 ns   413,533,26 ns   785,7143    357,1429     -      4800000 B   
             */
        [Benchmark]
        public void MutipleMessage_100_000()
        {
            for (int i = 0; i < 100_000; i++)
            {
                _ = ResourceQueue.Instance().PushItem(new DefaultResourceItem());
            }
            for (int i = 0; i < 100_000; i++)
            {
                ResourceQueue.Instance().Fetch<DefaultResourceItem>(item =>
                {

                    return true;
                }).Wait();
            }
        }
    }
}
