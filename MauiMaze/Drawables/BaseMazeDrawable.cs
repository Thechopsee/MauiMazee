using MauiMaze.Engine;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMaze.Drawables
{
    public class BaseMazeDrawable
    {
        public GameMaze maze { get; set; }
        public Player player { get; set; }
        
        public double cellWidth { get; set; }
        public double cellHeight { get; set; }
        protected bool inicialized{ get; set; }
        protected bool[,] walls { get; set; }

        public bool isInitialized() { return inicialized; }
        public Player reinitPlayer()
        {
            return new Player((int)maze.start.X, (int)maze.start.Y, cellWidth, cellHeight);
        }

        protected virtual void drawPlayer(ICanvas canvas) {
            float plX = (float)(player.positionX + player.playerSizeX);
            float plY = (float)(player.positionY + player.playerSizeY);
            canvas.StrokeColor = Colors.Orange;
            canvas.DrawCircle(plX, plY, MathF.Min((float)player.playerSizeX, ((float)player.playerSizeY))/3);
        }
        protected void drawStartAndEnd(ICanvas canvas)
        {
            canvas.StrokeColor = Colors.Blue;
            canvas.DrawCircle(maze.end.X, maze.end.Y, (float)Math.Min(cellWidth, cellHeight) / 3);
            canvas.StrokeColor = Colors.Red;
            canvas.DrawCircle(maze.start.X, maze.start.Y, (float)Math.Min(cellWidth, cellHeight) / 3);
            canvas.StrokeColor = Colors.Green;


        }
        protected virtual void drawPathFormaze(ICanvas canvas)
        {
            //just save off classic
            //TODO
            canvas.StrokeColor = Colors.Yellow;
            canvas.StrokeSize = 6;
            /*
            for (int i = 0; i < maze.path.Count - 1; i++)
            {
                int cell1 = maze.path[i];
                int cell2 = maze.path[i + 1];

                float x1 = (float)(cell1 % maze.Size.Width * cellWidth + cellWidth / 2);
                float y1 = (float)(Math.Floor((double)cell1 / mazee.Size.Width) * cellHeight + cellHeight / 2);
                float x2 = (float)(cell2 % maze.Size.Width * cellWidth + cellWidth / 2);
                float y2 = (float)(Math.Floor((double)cell2 / mazee.Size.Width) * cellHeight + cellHeight / 2);

                canvas.DrawLine(x1, y1, x2, y2);
            }
            */
        }

        public (bool,bool,bool) checkCollision(int x, int y, int x2, int y2)
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
                        if (walls[currX+1,currY] && walls[currX+2,currY])
                        {
                            collisionX = true;
                        }
                        if (walls[currX , currY+2] && walls[currX, currY+3])
                        {
                            collisionY = true;
                        }

                        return (true,collisionX,collisionY);

                    }
                }
            }

            return (false,false,false);
        }
    }
}
