using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMaze
{
    public class SettingsData
    {
        public int test { get; set; }
        public string language { get; set; }
        public bool SetsGenerator { get; set; }
        public SettingsData() { test = 8; SetsGenerator = true; }
    }
}
