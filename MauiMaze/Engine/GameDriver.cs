

using MauiMaze.Drawables;
using MauiMaze.Models;
using MauiMaze.Models.ClassicMaze;
using MauiMaze.Models.RoundedMaze;
using MauiMaze.Services;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MauiMaze.Engine
{
    internal class GameDriver
    {
        BaseMazeDrawable mazeDrawable;
        LoginCases gamestatus;
        public GraphicsView graphicsView { get; set; }
        private Player player { get; set; }
        private End end { get; set; }
        public bool ended { get; set; }
        private float lastposx {get;set;}
        private float lastposy { get; set; }
        public GameRecord gameRecord { get; set; }

        public void setPosition(float x, float y)
        {
            lastposx = x;
            lastposy = y;
        }

        public GameDriver(BaseMazeDrawable md,GraphicsView gv,int size,int mazetype,LoginCases gamestatus)
        {
            this.gamestatus = gamestatus;
            graphicsView = gv;
            mazeDrawable = md;
            mazeDrawable.player=new Player(0, 0,md.cellWidth,md.cellHeight);
            player = mazeDrawable.player;
            GameMaze maze;
            if (mazetype == 0)
            {
                maze = new Maze(new Size(size, size));
            }
            else
            {
                maze = new RoundedMaze(new Size(size,size));
            }
            mazeDrawable.maze=maze;
            graphicsView.Invalidate();
            if (this.gamestatus == LoginCases.Offline)
            {
                gameRecord = new GameRecord(-1,-1); //TODO
            }
            else
            {
                gameRecord = new GameRecord(maze.MazeID,UserDataProvider.GetInstance().getUserID());
            }
        }
        public GameDriver(BaseMazeDrawable md, GraphicsView gv, int size, int mazetype,Maze maze)
        {
            ended = false;
            graphicsView = gv;
            mazeDrawable = md;
            mazeDrawable.player = new Player(0, 0, md.cellWidth, md.cellHeight);
            player = mazeDrawable.player;
            mazeDrawable.maze = maze;
            graphicsView.Invalidate();
            if (this.gamestatus == LoginCases.Offline)
            {
                gameRecord = new GameRecord(-1, -1); //TODO
            }
            else
            {
                gameRecord = new GameRecord(maze.MazeID, UserDataProvider.GetInstance().getUserID());
            }
        }

        public void timerMove(object state)
        {
            movePlayerToPosition(lastposx, lastposy);
        }

        public void movePlayerToPosition(float x, float y)
        {
            if (player.positionX==0 && player.positionY==0)
            {
                mazeDrawable.reinitPlayer(player);
                end = mazeDrawable.maze.end;
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

            // Pokud vzdálenost je menší než poloměr, uživatel klikl na kolečko hráče
            if (distance < player.playerSizeX)
            {
                player.positionX = (float)(x - (player.playerSizeX));
                player.positionY = (float)(y - (player.playerSizeY));
                bool areHitted = false;
                if (player.checkFlushHit(oldHitbox,oldHitboy,oldPlayerX,oldPlayery,mazeDrawable))
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
            }
        }
        private void saveMove(bool hit)
        {
            double playerXPercentage = ((player.positionX + player.playerSizeX) / mazeDrawable.mazeWidth);
            double playerYPercentage = ((player.positionY + player.playerSizeY) / mazeDrawable.mazeHeight);
            double mazesize = mazeDrawable.maze.Size.Width;
            double xid = ((player.positionX + player.playerSizeX) / mazeDrawable.cellWidth);
            double yid = ((player.positionY + player.playerSizeY) / mazeDrawable.cellHeight);
            gameRecord.addCellMoveRecord((int)mazesize * (int)Math.Floor(yid) + (int)Math.Floor(xid));
            gameRecord.addMoveRecord(new MoveRecord(-1,(int)player.positionX, (int)player.positionY, playerXPercentage, playerYPercentage, hit));
        }
        private bool checkEnd()
        {
            return player.checkHit(end.X, end.Y, end.bottomX, end.bottomY);
        }
        private void endGameprocedure()
        {
            gameRecord.stopMeasuremnt();
            gameRecord.finished = true;
            //TODO //RecordRepository.GetInstance().addRecord(gameRecord);
            ended = true;
        }
        
    }
}
