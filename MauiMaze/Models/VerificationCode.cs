using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMaze.Models
{
    public class VerificationCode
    {
        public string Code{ get;  }
        public VerificationCode(string code)
        {
            this.Code = code;
        }
    }
}
