using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMaze.Engine
{
    public class MoveRecord
    {
        public int positionx;
        public double percentagex;
        public int positiony;
        public double percentagey;
        public bool hitWall;
        public MoveRecord(int positionx,int positiony, double percentagex, double percentagey, bool hitWall)
        {
            this.percentagex = percentagex;
            this.percentagey = percentagey;
            this.positionx = positionx;
            this.positiony = positiony; 
            this.hitWall = hitWall;
        }   
    }
}
