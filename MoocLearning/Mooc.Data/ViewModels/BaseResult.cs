using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mooc.Data.ViewModels
{
    public class BaseResult
    {
        public BaseResult()
        {

        }

        public BaseResult(int _code, string _msg)
        {
            this.code = _code;
            this.msg = _msg;
        }
        public int code { get; set; }

        public string msg { get; set; }
    }
}
