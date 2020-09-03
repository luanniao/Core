using System.IO;
using System.Text;
using System.Threading;
using Xunit.Abstractions;

namespace Z.LuanNiao.Service.Test
{

    public class CustomWriter : TextWriter
    {
        public override Encoding Encoding => Encoding.UTF8;
        private string _data = null;
        public string OutPutInfo = "";
        private readonly ITestOutputHelper _testOutput;
        private readonly AutoResetEvent _waitter = new AutoResetEvent(false);

        public CustomWriter(ITestOutputHelper testOutput)
        {
            this._testOutput = testOutput;
        }

        public void Wait()
        {
            _waitter.WaitOne();
        }
        public void SetData(string data)
        {
            _data = data;
        }
        public bool Result = false;

        public override void WriteLine(string value)
        {
            if (value==null)
            {
                return;
            }
            OutPutInfo = value;
            Result = value.Contains(_data);
            _testOutput.WriteLine("Console.Write:");
            _testOutput.WriteLine(value);
            _testOutput.WriteLine("Users guid data:");
            _testOutput.WriteLine(_data);
            _testOutput.WriteLine($"value.Contains:{value.Contains(_data)}");
            _testOutput.WriteLine($"value.IndexOf:{value.IndexOf(_data)!=-1}");
            _testOutput.WriteLine($"Result:{Result}");
            _waitter.Set();
        }
    }
}
