
using MauiMaze.Drawables;
using MauiMaze.Helpers;
using MauiMaze.Models;
using MauiMaze.Models.ClassicMaze;
using MauiMaze.Models.Generatory;
using MauiMaze.Models.RoundedMaze;
using MauiMaze.Services;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Plugin.LocalNotification.NotificationRequestGeofence;


namespace MauiMaze.Engine
{
    public class GameDriver
    {
        BaseMazeDrawable mazeDrawable;
        private GraphicsView graphicsView { get; set; }
        private Player player { get; set; }
        public bool ended { get; set; }
        private float lastposx {get;set;}
        private float lastposy { get; set; }
        public GameRecord gameRecord { get; set; }

        private static object syncObject = new object();
        private readonly object moveLock = new object();

        private long lastTime = 0;
        public int movecounter = 3;

        private void inicializeGameboard()
        {
            if (UserDataProvider.GetInstance().getUserID() > 0)
            {
                gameRecord = new GameRecord(mazeDrawable.maze.MazeID, UserDataProvider.GetInstance().getUserID());
            }
            else
            {
                gameRecord = new GameRecord(mazeDrawable.maze.MazeID, -1);
            }
            graphicsView.Invalidate();
        }
        public GameDriver(BaseMazeDrawable md,GraphicsView gv,int size,int mazetype,GeneratorEnum generator)
        {
            graphicsView = gv;
            mazeDrawable = md;
            mazeDrawable.player=new Player(0, 0,md.cellWidth,md.cellHeight);
            player = mazeDrawable.player;
            GameMaze maze;
            if (mazetype == 0)
            {

                maze = new Maze(size, size, Helpers.GeneratorEnum.Sets);
            }
            else
            {
                maze = new RoundedMaze(new Size(size,size),generator);
            }
            mazeDrawable.maze=maze;
            inicializeGameboard();
            
        }
        public GameDriver(BaseMazeDrawable md, RoundedMaze maze, GraphicsView gv)
        {
            graphicsView = gv;
            mazeDrawable = md;
            mazeDrawable.player = new Player(0, 0, md.cellWidth, md.cellHeight);
            player = mazeDrawable.player;
            mazeDrawable.maze = maze;
            inicializeGameboard();
        }
        public GameDriver(BaseMazeDrawable md, Maze maze, GraphicsView gv)
        {
            graphicsView = gv;
            mazeDrawable = md;
            mazeDrawable.player = new Player(0, 0, md.cellWidth, md.cellHeight);
            player = mazeDrawable.player;
            mazeDrawable.maze = maze;
            inicializeGameboard();
        }
        public void setPosition(float x, float y)
        {
            lastposx = x;
            lastposy = y;
        }
        public void timerMove(object state)
        {
            lock (moveLock)
            {
                movePlayerToPosition(lastposx, lastposy);
            }
        }
        public bool movePlayerToPosition(float x, float y)
        {
            
            if (player.positionX<=0 && player.positionY<=0)
            {
                mazeDrawable.reinitPlayer(player);
            }
            if (player.playerSizeX != mazeDrawable.cellWidth) {
                player.playerSizeX = mazeDrawable.cellWidth;
                player.playerSizeY =mazeDrawable.cellHeight;
            }


            int oldPlayerX = player.positionX;
            int oldPlayery = player.positionY;
            float oldHitbox = player.hitbox.X;
            float oldHitboy = player.hitbox.Y;
            
            double distance = Math.Sqrt(Math.Pow(x - player.hitbox.X, 2) + Math.Pow(y - player.hitbox.Y, 2));
            Monitor.Enter(syncObject);
            try
            {
                if (distance < player.playerSizeY)
                {
                    Player playercln = (Player)player.Clone();
                    playercln.positionX = (int)(x - (player.playerSizeX / 2));
                    playercln.positionY = (int)(y - (player.playerSizeY / 2));
                    bool areHitted = false;
                    (bool vysl, playercln) = checkTrajectory(oldHitbox, oldHitboy, oldPlayerX, oldPlayery, mazeDrawable.walls, playercln);
                    if (vysl)
                    {
                        areHitted = true;
                    }
                    player.positionX = playercln.positionX;
                    player.positionY = playercln.positionY;
  
                    if (checkEnd())
                    {
                        endGameprocedure();
                    }

                    mazeDrawable.player = player;
                    graphicsView.Invalidate();

                    saveMove(areHitted);
                    lastposx = -100;
                    lastposy = -100;
                    return true;
                }
            }
            finally
            {
                Monitor.Exit(syncObject);
            }

            return false;
        }

        public (bool, bool, bool) checkCollision(int x, int y, int x2, int y2, bool[,] walls)
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
                        return (true, collisionX, collisionY);

                    }
                }
            }
            return (false, false, false);
        }

        private void saveMove(bool hit)
        {
            
            if (movecounter <= 0)
            {
                double playerXPercentage = ((player.positionX + (player.playerSizeX / 2)) / mazeDrawable.mazeWidth);
                double playerYPercentage = ((player.positionY + (player.playerSizeY/2)) / mazeDrawable.mazeHeight);
                double mazesize = mazeDrawable.maze.Width;
                double xid = ((player.positionX + (player.playerSizeX / 2)) / mazeDrawable.cellWidth);
                double yid = ((player.positionY + (player.playerSizeY / 2)) / mazeDrawable.cellHeight);
                int cellID = (int)mazesize * (int)Math.Floor(yid) + (int)Math.Floor(xid);
                gameRecord.addCellMoveRecord(cellID);
                long now = gameRecord.stopwatch.ElapsedMilliseconds;
                int delta = (int)( now - lastTime);
                lastTime = now;
                gameRecord.addMoveRecord(new MoveRecord(-1, playerXPercentage, playerYPercentage, Convert.ToInt32(hit),delta,cellID));
                movecounter += 3;

            }
            else
            {
                movecounter--;
            }
        }
        private bool checkEnd()
        {
            return player.checkHit(mazeDrawable.maze.end.X, mazeDrawable.maze.end.Y, mazeDrawable.maze.end.bottomX, mazeDrawable.maze.end.bottomY);
        }
        (List<(int, int)>,int,int) Calculatepoints(int x0, int y0, int x1, int y1)
        {
            List<(int, int)> points = new List<(int, int)>();

            int dx = Math.Abs(x1 - x0);
            int dy = Math.Abs(y1 - y0);

            int stepx = x0 < x1 ? 1 : -1;
            int stepy = y0 < y1 ? 1 : -1;

            int err = dx - dy;

            int x = x0;
            int y = y0;

            while (true)
            {
                points.Add((x, y));

                if (x == x1 && y == y1) break;

                int e2 = 2 * err;

                if (e2 > -dy)
                {
                    err -= dy;
                    x += stepx;
                }

                if (e2 < dx)
                {
                    err += dx;
                    y += stepy;
                }
            }

            return (points,stepx,stepy);
        }
        ////old Good solution but not as good as actual
        //public (bool, Player) checkTrajectory(float oldHitbox, float oldHitboy, float oldPlayerX, float oldPlayerY, bool[,] walls, Player player)
        //{
        //    float xnew = player.hitbox.X;
        //    float ynew = player.hitbox.Y;
        //    float xorigin = oldHitbox;
        //    float yorigin = oldHitboy;


        //    float distance = (float)Math.Sqrt((xnew - xorigin) * (xnew - xorigin) + (ynew - yorigin) * (ynew - yorigin));

        //    int numberOfDots = (int)(distance);


        //    if (numberOfDots == 0)
        //    {
        //        return (false, player);
        //    }

        //    float xdiv = xnew - xorigin;
        //    float ydiv = ynew - yorigin;
        //    // float stepX=xdiv / distance;
        //    // float stepY= ydiv / distance;
        //    float stepX;
        //    float stepY;
        //    if (xdiv / distance < 0)
        //    {
        //        stepX = -1;
        //    }
        //    else if (xdiv / distance == 0)
        //    {
        //        stepX = 0;
        //    }
        //    else
        //    {
        //        stepX = 1;
        //    }
        //    if (ydiv / distance < 0)
        //    {
        //        stepY = -1;
        //    }
        //    else if (ydiv / distance == 0)
        //    {
        //        stepY = 0;
        //    }
        //    else
        //    {
        //        stepY = 1;
        //    }

        //    for (int i = 0; i <= numberOfDots - 1; i++)
        //    {
        //        int x = (int)(xorigin + i * stepX);
        //        int y = (int)(yorigin + i * stepY);


        //        (bool, bool, bool) hitcheck = checkCollision(x, y, (int)(x + player.hitbox.Size), (int)(y + player.hitbox.Size), walls);
        //        if (hitcheck.Item1)
        //        {
        //            float xrep = (xorigin + (i - 1) * stepX) + (stepX * (hitcheck.Item3 ? 0 : 3));
        //            float yrep = (yorigin + (i - 1) * stepY) + (stepY * (hitcheck.Item2 ? 0 : 3));
        //            player.recalculateHitbox();
        //            (bool, bool, bool) hitcheck2 = checkCollision((int)xrep, (int)yrep, (int)(xrep + player.hitbox.Size - 1), (int)(yrep + player.hitbox.Size - 1), walls);
        //            player.recalculateHitbox();
        //            if (hitcheck2.Item1)
        //            {
        //                player.positionX = oldPlayerX;
        //                player.positionY = oldPlayerY;
        //                return (true, player);
        //            }

        //            float minsize = (float)Math.Min(player.playerSizeX, player.playerSizeY);
        //            player.positionX = xrep - (float)(player.playerSizeX - (minsize / 1.5f)) / 2;
        //            player.positionY = yrep - (float)(player.playerSizeY - (minsize / 1.5f)) / 2;
        //            return (true, player);
        //        }

        //    }

        //    return (false, player);

        //}

        public (bool, Player) checkTrajectory(float oldHitbox, float oldHitboy, int oldPlayerX, int oldPlayerY, bool[,] walls, Player player)
        {
            float xnew = player.hitbox.X;
            float ynew = player.hitbox.Y;
            float xorigin = oldHitbox;
            float yorigin = oldHitboy;

            float distance = (float)Math.Sqrt((xnew - xorigin) * (xnew - xorigin) + (ynew - yorigin) * (ynew - yorigin));

            (List<(int, int)> points,int stepx,int stepy) = Calculatepoints((int)xorigin, (int)yorigin, (int)xnew, (int)ynew);

            for (int i = 0; i <= points.Count - 1; i++)
            {
                int x = points[i].Item1;
                int y = points[i].Item2;


                (bool, bool, bool) hitcheck = checkCollision(x-1, y-1, (int)(x + player.hitbox.Size)+1, (int)(y + player.hitbox.Size)+1, walls);
                if (hitcheck.Item1)
                {
                    float xrep = x + (hitcheck.Item3 ? 0 : 3)*stepy;
                    float yrep = y + (hitcheck.Item2 ? 0 : 3)*stepx;
                    (bool, bool, bool) hitcheck2 = checkCollision((int)xrep, (int)yrep, (int)(xrep + player.hitbox.Size), (int)(yrep + player.hitbox.Size), walls);
                    if (true)
                    {
                        if (i !=0)
                        {
                            player.positionX = oldPlayerX;
                            player.positionY = oldPlayerY;
                        }
                        else
                        {
                            player.positionX = oldPlayerX;
                            player.positionY = oldPlayerY;
                        }
                        return (true, player);
                    }
                    else
                    {
                        player.positionX = (int)xrep - (int)((player.playerSizeX - player.hitbox.Size) / 2);
                        player.positionY = (int)yrep - (int)((player.playerSizeY - player.hitbox.Size) / 2);
                        return (true, player);
                    }


                }

            }
            return (false, player);
        }
        private void endGameprocedure()
        {
            gameRecord.stopMeasuremnt();
            gameRecord.finished = true;
            ended = true;
        }
        
    }
}
