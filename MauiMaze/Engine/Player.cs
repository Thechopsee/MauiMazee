using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMaze.Engine
{
    public class Player
    {
        public float positionX { get; set; }
        public float positionY { get; set; }

        public double playerSizeX { get; set; }
        public double playerSizeY { get; set; }

        public Player(int positionx,int positiony,double playerSizeX,double SizeY)
        {
            this.positionX = positionx;
            this.positionY = positiony;
            this.playerSizeX = playerSizeX;
            this.playerSizeY = playerSizeY;
        }
    }
}
