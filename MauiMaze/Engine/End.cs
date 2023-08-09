using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMaze.Engine
{
    public class End
    {
        public int X { get; }
        public int Y { get; }
        public int bottomX { get; }
        public int bottomY { get; }
        public End(int x, int y,int bottomX,int bottomY)
        {
            X = x;
            Y = y;
            this.bottomX = bottomX;
            this.bottomY = bottomY;  
        }   
    }
}
