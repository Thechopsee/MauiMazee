using MauiMaze.Drawables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MauiMaze.Engine
{
    public class Player : ICloneable
    {
        public bool dummy { get; set; }
        private int _positionX;
        public int positionX
        {
            get { return _positionX; }
            set
            {
                _positionX = value;
                recalculateHitbox();
            }
        }

        private int _positionY;
        public int positionY
        {
            get { return _positionY; }
            set
            {
                _positionY = value;
                recalculateHitbox();
            }
        }

        public double playerSizeX { get; set; }
        public double playerSizeY { get; set; }

        public Hitbox hitbox { get; set; }

        public bool checkHit(float X, float Y, float Xbottom, float Ybottom)
        {
            return (hitbox.X < Xbottom && hitbox.Y < Ybottom && hitbox.X > X && hitbox.Y > Y);
        }

        public void recalculateHitbox()
        {
            float minsize = MathF.Min((float)playerSizeX, (float)playerSizeY);
            hitbox.X = positionX + (float)(playerSizeX - (minsize / 1.5f)) / 2;
            hitbox.Y = positionY + (float)(playerSizeY - (minsize / 1.5f)) / 2;
            hitbox.Size = minsize / 1.5f;
        }

        public void reInit(int x, int y, double sx, double sy)
        {
            positionX = x;
            positionY = y;
            playerSizeX = sx;
            playerSizeY = sy;
        }

        public Player(int positionx, int positiony, double playerSizeX, double playerSizeY)
        {
            this.hitbox = new Hitbox();
            this.positionX = positionx;
            this.positionY = positiony;
            this.playerSizeX = playerSizeX;
            this.playerSizeY = playerSizeY;
        }

        public object Clone()
        {
            return new Player((int)this.positionX, (int)this.positionY, this.playerSizeX, this.playerSizeY)
            {
                dummy = this.dummy,
                hitbox = new Hitbox()
                {
                    X = this.hitbox.X,
                    Y = this.hitbox.Y,
                    Size = this.hitbox.Size
                }
            };
        }
    }
}
