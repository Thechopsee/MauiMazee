using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMaze.Engine
{
    internal class MoveRecord
    {
        public int positionx;
        public int positiony;
        public bool hitWall;
        public MoveRecord(int positionx,int positiony, bool hitWall)
        {
            this.positionx = positionx;
            this.positiony = positiony; 
            this.hitWall = hitWall;
        }   
    }
}
