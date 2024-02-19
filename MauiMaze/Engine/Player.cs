using MauiMaze.Drawables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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
            return (hitbox.X < Xbottom && hitbox.Y < Ybottom && hitbox.X > X && hitbox.Y > Y);

        }
        public (bool, bool, bool) checkCollision(int x, int y, int x2, int y2,bool[,] walls)
        {
            int startX = Math.Min(x, x2);
            int startY = Math.Min(y, y2);
            int endX = Math.Max(x, x2);
            int endY = Math.Max(y, y2);
            bool collisionX = false;
            bool collisionY = false;
            for (int currX = startX; currX <= endX; currX++)
            {
                for (int currY = startY; currY <= endY; currY++)
                {
                    if (currX >= 0 && currX < walls.GetLength(0) &&
                        currY >= 0 && currY < walls.GetLength(1) &&
                        walls[currX, currY])
                    {
                        if (walls[currX, currY + 1] || walls[currX, currY - 1])
                        {
                            collisionY = true;
                        }
                        if (walls[currX + 1, currY] || walls[currX - 1, currY])
                        {
                            collisionX = true;
                        }
                        if ((currY + 5 > endY || currY < startY + 5) && collisionY)
                        {
                            if ((!walls[currX, currY + 1] && !walls[currX, currY + 2]) || (!walls[currX, currY - 1] && !walls[currX, currY - 2]))
                            {
                                collisionX = true;
                                collisionY = false;
                            }
                        }
                        if ((currX + 5 > endX || currX < startX + 5) && collisionX)
                        {
                            if ((!walls[currX + 1, currY] && !walls[currX + 2, currY]) || (!walls[currX - 1, currY] && !walls[currX - 2, currY]))
                            {

                                collisionX = false;
                                collisionY = true;
                            }

                        }
                        return (true, collisionX, collisionY);

                    }
                }
            }
            return (false, false, false);
        }

        public bool checkFlushHit(float oldHitbox,float oldHitboy, float oldPlayerX,float oldPlayerY, bool[,] walls)
        {
            float xnew = hitbox.X;
            float ynew = hitbox.Y;
            float xorigin = oldHitbox;
            float yorigin = oldHitboy;


            float distance = (float)Math.Sqrt((xnew - xorigin) * (xnew - xorigin) + (ynew - yorigin) * (ynew - yorigin));

            int numberOfDots = (int)(distance);


            if (numberOfDots == 0)
            {
                return false;
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

            for (int i = 0; i <= numberOfDots-1; i++)
            {
                int x = (int)(xorigin + i * stepX);
                int y = (int)(yorigin + i * stepY);


                (bool, bool, bool) hitcheck = checkCollision(x, y, (int)(x + hitbox.Size), (int)(y + hitbox.Size),walls);
                if (hitcheck.Item1)
                {
                    float xrep = (xorigin + (i-1) * stepX) + (stepX*(hitcheck.Item3?0:3));
                    float yrep = (yorigin + (i-1) * stepY) + (stepY*(hitcheck.Item2?0:3));
                    recalculateHitbox();
                    (bool, bool, bool) hitcheck2 = checkCollision((int)xrep, (int)yrep, (int)(xrep + hitbox.Size-1), (int)(yrep + hitbox.Size-1),walls);
                    recalculateHitbox();
                    if (hitcheck2.Item1 )
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
        public void reInit(int x,int y,double sx,double sy)
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
    }

}
