using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMaze.Helpers
{
    public  class CellData
    {
        public int num { get; set; }
        public Color color { get; set; }
        public int time { get; set; }
        public int hit { get; set; }
        
        public CellData(int num,Color color,int time,int hit) {
            this.num = num;
            this.color = color;
            this.time = time;
            this.hit = hit;
        }
    }
}
