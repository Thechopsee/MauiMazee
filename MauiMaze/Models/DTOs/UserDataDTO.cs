using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMaze.Models.DTOs
{
    public class UserDataDTO
    {
        public int id { get; set; }
        public int role { get; set; }
        public string email { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string code { get; set; }

    }
}
