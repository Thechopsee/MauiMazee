
using MauiMaze.Drawables;
using MauiMaze.Models;
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
        MazeDrawable mazeDrawable;
        List<MoveRecord> moveRecords;
        public GraphicsView graphicsView { get; set; }
        Player player;
        End end;
        public bool ended {get;set;}
        public GameRecord gameRecord { get; set; }
        int counter = 300;

        public GameDriver(MazeDrawable md,GraphicsView gv,int size)
        {
            ended = false;
            graphicsView = gv;
            mazeDrawable = md;
            mazeDrawable.setPlayer(new Player(0, 0,md.cellWidth,md.cellHeight));
            this.moveRecords = new List<MoveRecord>();
            player = mazeDrawable.GetPlayer();
            Maze maze = new Maze(new Size(size, size));
            mazeDrawable.setNewMaze(maze);
            graphicsView.Invalidate();
            gameRecord = new GameRecord("test"+1,1); //TODO
            

        }

        public void movePlayerToPosition(float x, float y)
        {
            end = mazeDrawable.end;
            if (player.playerSizeX != mazeDrawable.cellWidth/2) {
                player.playerSizeX = mazeDrawable.cellWidth/2;
                player.playerSizeY =mazeDrawable.cellHeight/2;
            }
            // Vypočítání vzdálenosti mezi pozicí kliku (x, y) a středem hráče (player.positionX, player.positionY)
            double distance = Math.Sqrt(Math.Pow((x - (player.playerSizeX/2)) - player.positionX, 2) + Math.Pow((y-(player.playerSizeY/2)) - player.positionY, 2));
            //Application.Current.MainPage.DisplayAlert("Upozornění", "distance "+distance+" size "+player.playerSizeX, "OK");

            // Porovnání vzdálenosti s poloměrem hráče (player.playerSizeX)
            // Pokud vzdálenost je menší než poloměr, uživatel klikl na kolečko hráče
            if (distance < player.playerSizeX)
            {
                if (!mazeDrawable.checkCollision((int)player.positionX, (int)player.positionY, (int)(player.positionX + player.playerSizeX), (int)(player.positionY+ player.playerSizeY)))
                {
                    //TODO test
                    player.positionX = (float)(x - (player.playerSizeX));
                    player.positionY = (float)(y - (player.playerSizeY));
                    if (checkEnd())
                    { 
                        Application.Current.MainPage.DisplayAlert("Upozornění", "Konec hry", "OK");
                        endGameprocedure();
                    }
                    mazeDrawable.setPlayer(player);
                    gameRecord.addMoveRecord(new MoveRecord((int)player.positionX, (int)player.positionY,false));
                    graphicsView.Invalidate();
                }
            }
        }
        private bool checkEnd()
        {   
            if (player.positionX > end.X && player.positionY > end.Y && player.positionX < end.bottomX && player.positionY < end.bottomY)
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
