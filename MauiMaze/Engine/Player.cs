using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMaze.Engine
{
    internal class Player
    {
        public int positionX { get; set; }
        public int positionY { get; set; }

        public Player(int positionx,int positiony)
        {
            this.positionX = positionx;
            this.positionY = positiony;
        }
    }
}
