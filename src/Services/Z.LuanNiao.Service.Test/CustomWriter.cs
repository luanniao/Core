using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z.LuanNiao.Service.Test
{
    public class CustomWriter : TextWriter
    {
        public override Encoding Encoding => throw new NotImplementedException();
        private string _data = null;
        public string OutPutInfo = "";
        public void SetData(string data)
        {
            _data = data;
        }
        public bool Result = false;

        public override void WriteLine(string value)
        {
            OutPutInfo = value;
            Result = value == null ? false : value.Contains(_data);
        }
    }
}
