using MauiMaze.Drawables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMaze.Engine
{
    public class Player
    {
        private float _positionX;
        public float positionX
        {
            get { return _positionX; }
            set
            {
                _positionX = value;
                recalculateHitbox();
            }
        }

        private float _positionY;
        public float positionY
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

        public bool checkHit(float X,float Y,float Xbottom ,float Ybottom)
        {
            if (hitbox.X < Xbottom && hitbox.Y < Ybottom && hitbox.X > X && hitbox.Y > Y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool checkFlushHit(float oldHitbox,float oldHitboy, float oldPlayerX,float oldPlayerY, BaseMazeDrawable mazeDrawable)
        {
            float xnew = hitbox.X;
            float ynew = hitbox.Y;
            float xorigin = oldHitbox;
            float yorigin = oldHitboy;


            float distance = (float)Math.Sqrt((xnew - xorigin) * (xnew - xorigin) + (ynew - yorigin) * (ynew - yorigin));

            int numberOfDots = (int)(distance);
            if (numberOfDots == 0)
            {
                numberOfDots = 1;
            }
            float xdiv = xnew - xorigin;
            float ydiv = ynew - yorigin;
            // float stepX=xdiv / distance;
            // float stepY= ydiv / distance;
            float stepX;
            float stepY;
            if (xdiv / distance < 0)
            {
                stepX = -1;
            }
            else if (xdiv / distance == 0)
            {
                stepX = 0;
            }
            else
            {
                stepX = 1;
            }
            if (ydiv / distance < 0)
            {
                stepY = -1;
            }
            else if (ydiv / distance == 0)
            {
                stepY = 0;
            }
            else
            {
                stepY = 1;
            }

            for (int i = 0; i <= numberOfDots; i++)
            {
                int x = (int)(xorigin + i * stepX);
                int y = (int)(yorigin + i * stepY);
                // Application.Current.MainPage.DisplayAlert("Upozornění", "x" + x + " y" + y + "dista" + distance + "\nnew" + xnew + " " + ynew + "\norigin" + xorigin + " " + yorigin + "\n" + stepX + " " + stepY, "OK"); ;

                (bool, bool, bool) hitcheck = mazeDrawable.checkCollision(x, y, (int)(x + hitbox.Size), (int)(y + hitbox.Size));
                if (hitcheck.Item1)
                {
                    float xrep = (xorigin + (i-1) * stepX) + (stepX*(hitcheck.Item3?0:1));
                    float yrep = (yorigin + (i-1) * stepY) + (stepY*(hitcheck.Item2?0:1));
                    recalculateHitbox();
                    (bool, bool, bool) hitcheck2 = mazeDrawable.checkCollision((int)xrep, (int)yrep, (int)(xrep + hitbox.Size-1), (int)(yrep + hitbox.Size-1));
                    recalculateHitbox();
                    if (hitcheck2.Item1)
                    {
                        positionX = oldPlayerX;
                        positionY = oldPlayerY;
                        return true;
                    }

                    float minsize = MathF.Min((float)playerSizeX, (float)playerSizeY);
                    positionX = xrep - ((float)playerSizeX - (minsize / 1.5f) / 2);
                    positionY = yrep- ((float)playerSizeY - (minsize / 1.5f) / 2);
                    return true;
                }

            }

            return false;

        }

        private void recalculateHitbox()
        {
            float minsize = MathF.Min((float)playerSizeX, (float)playerSizeY);
            hitbox.X = positionX + (float)playerSizeX - (minsize / 1.5f) / 2;
            hitbox.Y = positionY + (float)playerSizeY - (minsize / 1.5f) / 2;
            hitbox.Size = minsize / 1.5f;
        }

        public Player(int positionx, int positiony, double playerSizeX, double playerSizeY)
        {
            this.hitbox = new Hitbox();
            this.positionX = positionx;
            this.positionY = positiony;
            this.playerSizeX = playerSizeX;
            this.playerSizeY = playerSizeY;
        }
    }

}
