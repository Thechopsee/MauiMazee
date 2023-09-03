
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
        int counter = 300;

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

        public void movePlayerToPosition(float x, float y)
        {
            end = mazeDrawable.maze.end;
            mazeDrawable.maze.firstrun = true;
            //Application.Current.MainPage.DisplayAlert("Upozornění", "Touch" +player.playerSizeX+" "+player.playerSizeY+" "+player.positionX+" "+player.positionY, "OK");
            if (player.positionX==0 && player.positionY==0)
            {

                player = mazeDrawable.reinitPlayer();
            }
            if (player.playerSizeX != mazeDrawable.cellWidth/2) {
                player.playerSizeX = mazeDrawable.cellWidth/2;
                player.playerSizeY =mazeDrawable.cellHeight/2;
            }
            float oldPlayerX = player.positionX;
            float oldPlayery = player.positionY;
            // Vypočítání vzdálenosti mezi pozicí kliku (x, y) a středem hráče (player.positionX, player.positionY)
            double distance = Math.Sqrt(Math.Pow((x - (player.playerSizeX/2)) - player.positionX, 2) + Math.Pow((y-(player.playerSizeY/2)) - player.positionY, 2));

            // Pokud vzdálenost je menší než poloměr, uživatel klikl na kolečko hráče
            if (distance < player.playerSizeX)
            {
                player.positionX = (float)(x - (player.playerSizeX));
                player.positionY = (float)(y - (player.playerSizeY));
                if (mazeDrawable.checkCollision((int)(player.positionX + (player.playerSizeX / 1.5)), (int)player.positionY+ (int)(player.playerSizeY / 2), (int)(player.positionX + ((player.playerSizeX/2)+player.playerSizeX/1.5)), (int)(player.positionY + player.playerSizeY)+(int)(player.playerSizeY / 2)) )
                {
                    player.positionX = oldPlayerX;
                    player.positionY = oldPlayery;
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
            if (player.positionX > end.bottomX && player.positionY > end.bottomY && player.positionX < end.X && player.positionY < end.Y)
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
    }
}
