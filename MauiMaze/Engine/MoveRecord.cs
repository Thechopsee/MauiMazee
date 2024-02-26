using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMaze.Engine
{
    public class MoveRecord
    {
        public readonly int mrID;
        public readonly double percentagex;
        public readonly double percentagey;
        public readonly int hitWall;
        public readonly int deltaTinMilisec;
        public readonly int cell;
        public MoveRecord(int mrID, double percentagex, double percentagey, int hitWall,int deltaTinMilisec, int cell)
        {
            this.percentagex = percentagex;
            this.percentagey = percentagey;

            this.hitWall = hitWall;
            this.deltaTinMilisec = deltaTinMilisec;
            this.cell = cell;
        }
        
    }
}
