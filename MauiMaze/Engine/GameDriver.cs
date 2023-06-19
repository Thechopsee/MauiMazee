
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
        Maze maze;
        Player player;
        List<MoveRecord> moveRecords;

        public GameDriver(Maze maze)
        {
            this.maze = maze;
            int[] pos = maze.getPositionsOfStart();
            player = new Player(pos[0], pos[1]);
            this.moveRecords = new List<MoveRecord>();
        }
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

       
    }
}
