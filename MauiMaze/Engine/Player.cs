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

        public Hitbox hitbox { get; set; }
        public void recalculateHitbox() 
        {
            float minsize = MathF.Min((float)playerSizeX, ((float)playerSizeY));
            hitbox.X = positionX + (float)playerSizeX - (minsize / 1.5f) / 2;
            hitbox.Y = positionY + (float)playerSizeY - (minsize / 1.5f) / 2;
            hitbox.size = minsize / 1.5f;
        }

        public Player(int positionx,int positiony,double playerSizeX,double playerSizeY)
        {
            this.hitbox = new Hitbox();
            this.positionX = positionx;
            this.positionY = positiony;
            this.playerSizeX = playerSizeX;
            this.playerSizeY = playerSizeY;
        }
    }
}
