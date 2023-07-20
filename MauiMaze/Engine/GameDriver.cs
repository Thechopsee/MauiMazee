
using MauiMaze.Drawables;
using MauiMaze.Models;
using System;
using System.Collections.Generic;
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

        public GameDriver(MazeDrawable md,GraphicsView gv)
        {
            graphicsView = gv;
            mazeDrawable = md;
            mazeDrawable.setPlayer(new Player(0, 0,md.cellWidth,md.cellHeight));
            this.moveRecords = new List<MoveRecord>();
            player = mazeDrawable.GetPlayer();
            Maze maze = new Maze(new Size(10, 10));
            mazeDrawable.setNewMaze(maze);
            graphicsView.Invalidate();

        }

        public void movePlayerToPosition(float x, float y)
        {
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
                player.positionX = (float)(x - (player.playerSizeX ));
                player.positionY = (float)(y - (player.playerSizeY ));
                mazeDrawable.setPlayer(player);
                graphicsView.Invalidate();
            }
        }

        /*
        public void movePlayerTop()
        {
            if(player.positionY>0)
            {
                if (maze.isThereWall(player.positionX, player.positionY, player.positionX, player.positionY - 1))
                {
                    moveRecords.Add(new MoveRecord(player.positionX, player.positionY, true));
                }
                else
                {
                    moveRecords.Add(new MoveRecord(player.positionX,player.positionY-1,false));
                    player.positionY = player.positionY-1;
                }
            }
        }
        public void movePlayerBottom()
        {
            if (player.positionY < maze.Size.Height-1)
            {
                if (maze.isThereWall(player.positionX, player.positionY, player.positionX, player.positionY + 1))
                {
                    moveRecords.Add(new MoveRecord(player.positionX, player.positionY, true));
                }
                else
                {
                    moveRecords.Add(new MoveRecord(player.positionX, player.positionY + 1, false));
                    player.positionY = player.positionY + 1;
                }
            }
        }
        public void movePlayerLeft()
        {
            if (player.positionX >0)
            {
                if (maze.isThereWall(player.positionX, player.positionY, player.positionX-1, player.positionY))
                {
                    moveRecords.Add(new MoveRecord(player.positionX, player.positionY, true));
                }
                else
                {
                    moveRecords.Add(new MoveRecord(player.positionX-1, player.positionY, false));
                    player.positionX = player.positionX - 1;
                }
            }
        }
        public void movePlayerRight()
        {
            if (player.positionX < maze.Size.Width - 1)
            {
                if (maze.isThereWall(player.positionX, player.positionY, player.positionX+1, player.positionY))
                {
                    moveRecords.Add(new MoveRecord(player.positionX, player.positionY, true));
                }
                else
                {
                    moveRecords.Add(new MoveRecord(player.positionX + 1, player.positionY, false));
                    player.positionX = player.positionX + 1;
                }
            }
        }
        */

    }
}
