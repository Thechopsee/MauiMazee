

using MauiMaze.Drawables;
using MauiMaze.Models;
using MauiMaze.Models.ClassicMaze;
using MauiMaze.Models.RoundedMaze;
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
        List<MoveRecord> moveRecords;
        public GraphicsView graphicsView { get; set; }
        Player player;
        End end;
        public bool ended {get;set;}
        public GameRecord gameRecord { get; set; }

        public GameDriver(BaseMazeDrawable md,GraphicsView gv,int size,int mazetype)
        {
            ended = false;
            graphicsView = gv;
            mazeDrawable = md;
            mazeDrawable.player=new Player(0, 0,md.cellWidth,md.cellHeight);
            this.moveRecords = new List<MoveRecord>();
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
            gameRecord = new GameRecord("test"+1,1); //TODO
        }
        public GameDriver(BaseMazeDrawable md, GraphicsView gv, int size, int mazetype,Maze maze)
        {
            ended = false;
            graphicsView = gv;
            mazeDrawable = md;
            mazeDrawable.player = new Player(0, 0, md.cellWidth, md.cellHeight);
            this.moveRecords = new List<MoveRecord>();
            player = mazeDrawable.player;
            mazeDrawable.maze = maze;
            graphicsView.Invalidate();
            gameRecord = new GameRecord("test" + 1, 1); //TODO
        }

        public void movePlayerToPosition(float x, float y)
        {
            end = mazeDrawable.maze.end;
            mazeDrawable.maze.firstrun = true;
            //Application.Current.MainPage.DisplayAlert("Upozornění", "Touch" +player.playerSizeX+" "+player.playerSizeY+" "+player.positionX+" "+player.positionY, "OK");
            if (player.positionX==0 && player.positionY==0)
            {

                player = mazeDrawable.reinitPlayer();
            }
            if (player.playerSizeX != mazeDrawable.cellWidth) {
                player.playerSizeX = mazeDrawable.cellWidth;
                player.playerSizeY =mazeDrawable.cellHeight;
            }
            float oldPlayerX = player.positionX;
            float oldPlayery = player.positionY;
            
            double distance = Math.Sqrt(Math.Pow((x - (player.playerSizeX/2)) - player.positionX, 2) + Math.Pow((y-(player.playerSizeY/2)) - player.positionY, 2));

            // Pokud vzdálenost je menší než poloměr, uživatel klikl na kolečko hráče
            if (distance < player.playerSizeX)
            {
                player.positionX = (float)(x - (player.playerSizeX));
                player.positionY = (float)(y - (player.playerSizeY));
                player.recalculateHitbox();
                if (mazeDrawable.checkCollision((int)player.hitbox.X, (int)player.hitbox.Y, (int)(player.hitbox.X + player.hitbox.size), (int)(player.hitbox.Y + player.hitbox.size)))
                {


                    if (player.positionX > oldPlayerX)
                    {
                        player.positionX = oldPlayerX ;
                    }
                    else
                    {
                        player.positionX = oldPlayerX ;
                    }
                    if (player.positionY > oldPlayery)
                    {
                        player.positionY = oldPlayery ;
                    }
                    else
                    {
                        player.positionY = oldPlayery ;
                    }

                    return;
                }
                if (checkEnd())
                { 
                    Application.Current.MainPage.DisplayAlert("Upozornění", "Konec hry", "OK");
                    endGameprocedure();
                }
                mazeDrawable.player = player;
                gameRecord.addMoveRecord(new MoveRecord((int)player.positionX, (int)player.positionY,false));
                graphicsView.Invalidate(); 
            }
        }
        private bool checkEnd()
        {
            player.recalculateHitbox();
            if (player.hitbox.X> end.bottomX && player.hitbox.Y > end.bottomY && player.hitbox.X < end.X && player.hitbox.Y < end.Y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void endGameprocedure()
        {
            gameRecord.stopMeasuremnt();
            RecordRepository.GetInstance().addRecord(gameRecord);
            ended = true;
            //go out

        }
        private bool checkFlush(float oldPlayerX, float oldPlayerY)
        {
            int xnew = (int)player.positionX;
            int ynew = (int)player.positionY;
            int xorigin = (int)oldPlayerX ;
            int yorigin = (int)oldPlayerY ;
            if (player.positionY < 0 || player.positionX<0)
            {
                return false;
                
            }

            float distance = (float)Math.Sqrt((xnew - xorigin) * (xnew - xorigin) + (ynew - yorigin) * (ynew - yorigin));

            int numberOfDots = (int)(distance);
            if (numberOfDots == 0)
            {
                return false;
            }
            float stepX = (xnew - xorigin)/numberOfDots;
            float stepY = (ynew - yorigin)/numberOfDots;

            for (int i = 0; i <= numberOfDots; i++)
            {
                int x = (int)(xorigin + i * stepX);
                int y = (int)(yorigin + i * stepY);
                if (mazeDrawable.checkCollision(x, y, (int)(x + player.hitbox.size), (int)(y + player.hitbox.size)))
                {
                    //Application.Current.MainPage.DisplayAlert("Upozornění", "x" + x + " y" + y + "dista" + distance + "\nnew"+xnew+" "+ynew+"\norigin"+xorigin+" "+yorigin+"\n"+stepX+" "+stepY, "OK"); ;
                    return true;
                }

            }
            return false;
           
        }
    }
}
