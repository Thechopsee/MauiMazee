

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
            graphicsView.Invalidate();
            movePlayerToPosition(lastposx, lastposy);
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


            float oldPlayerX = player.positionX;
            float oldPlayery = player.positionY;
            float oldHitbox = player.hitbox.X;
            float oldHitboy = player.hitbox.Y;
            
            double distance = Math.Sqrt(Math.Pow((x - (player.playerSizeX/2)) - player.positionX, 2) + Math.Pow((y-(player.playerSizeY/2)) - player.positionY, 2));

            if (distance < player.playerSizeX)
            {
                player.positionX = (float)(x - (player.playerSizeX));
                player.positionY = (float)(y - (player.playerSizeY));
                bool areHitted = false;
                (bool vysl, player) = checkTrajectory(oldHitbox, oldHitboy, oldPlayerX, oldPlayery, mazeDrawable.walls, player);
                if (vysl)
                {
                    areHitted = true;
                    graphicsView.Invalidate();
                }
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

        private void saveMove(bool hit)
        {
            
            if (movecounter <= 0)
            {
                double playerXPercentage = ((player.positionX + player.playerSizeX) / mazeDrawable.mazeWidth);
                double playerYPercentage = ((player.positionY + player.playerSizeY) / mazeDrawable.mazeHeight);
                double mazesize = mazeDrawable.maze.Width;
                double xid = ((player.positionX + player.playerSizeX) / mazeDrawable.cellWidth);
                double yid = ((player.positionY + player.playerSizeY) / mazeDrawable.cellHeight);
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
        public (bool,Player) checkTrajectory(float oldHitbox, float oldHitboy, float oldPlayerX, float oldPlayerY, bool[,] walls,Player player)
        {
            float xnew = player.hitbox.X;
            float ynew = player.hitbox.Y;
            float xorigin = oldHitbox;
            float yorigin = oldHitboy;


            float distance = (float)Math.Sqrt((xnew - xorigin) * (xnew - xorigin) + (ynew - yorigin) * (ynew - yorigin));

            int numberOfDots = (int)(distance);


            if (numberOfDots == 0)
            {
                return (false,player);
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

            for (int i = 0; i <= numberOfDots - 1; i++)
            {
                int x = (int)(xorigin + i * stepX);
                int y = (int)(yorigin + i * stepY);


                (bool, bool, bool) hitcheck = checkCollision(x, y, (int)(x + player.hitbox.Size), (int)(y + player.hitbox.Size), walls);
                if (hitcheck.Item1)
                {
                    float xrep = (xorigin + (i - 1) * stepX) + (stepX * (hitcheck.Item3 ? 0 : 3));
                    float yrep = (yorigin + (i - 1) * stepY) + (stepY * (hitcheck.Item2 ? 0 : 3));
                    player.recalculateHitbox();
                    (bool, bool, bool) hitcheck2 = checkCollision((int)xrep, (int)yrep, (int)(xrep + player.hitbox.Size - 1), (int)(yrep + player.hitbox.Size - 1), walls);
                    player.recalculateHitbox();
                    if (hitcheck2.Item1)
                    {
                        player.positionX = oldPlayerX;
                        player.positionY = oldPlayerY;
                        return (true, player);
                    }

                    float minsize = (float)Math.Min(player.playerSizeX, player.playerSizeY);
                    player.positionX = xrep - ((float)player.playerSizeX - (minsize / 1.5f) / 2);
                    player.positionY = yrep - ((float)player.playerSizeY - (minsize / 1.5f) / 2);
                    return (true, player);
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
