using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMaze.Engine
{
    public class Start
    {
        public int X { get; }
        public int Y { get; }
        public int cell { get; set; }
        public Start(int X, int Y,int cell)
        {
            this.X = X;
            this.Y = Y;
            this.cell = cell;
        }
        
    }
}
