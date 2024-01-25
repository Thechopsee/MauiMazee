using MauiMaze.Engine;
using MauiMaze.Exceptions;
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

        public double mazeWidth { get; set; }
        public double mazeHeight { get; set; }
        public double cellWidth { get; set; }
        public double cellHeight { get; set; }
        protected bool[,] walls { get; set; }

        public bool showCell {get;set;}
        public bool showAll { get; set; }
        public int actualID { get; set; }
        public Player preview { get; set; }
        public IEnumerable<GameRecord> gameRecords { get; set; }

        public void reinitPlayer(Player player)
        {
            if (player is null) { throw new ArgumentNullException(); };
            player.reInit((int)maze.start.X, (int)maze.start.Y + 2,cellWidth,cellHeight);
            
        }
        protected virtual void drawPlayer(ICanvas canvas) {
            if (canvas is null)
            {
                throw new CanvasNotAvailableExpectation("");
            }
            float plX = (float)(player.positionX + player.playerSizeX);
            float plY = (float)(player.positionY + player.playerSizeY);
            canvas.StrokeColor = Colors.Orange;
            canvas.DrawCircle(plX, plY, MathF.Min((float)player.playerSizeX, ((float)player.playerSizeY))/3);
        }
        protected virtual void drawPreview(ICanvas canvas)
        {
            if (canvas is null)
            {
                throw new CanvasNotAvailableExpectation("");
            }
            float plX = (float)(preview.positionX + preview.playerSizeX);
            float plY = (float)(preview.positionY + preview.playerSizeY);
            canvas.StrokeColor = Colors.Orange;
            canvas.DrawCircle(plX, plY, MathF.Min((float)preview.playerSizeX, ((float)preview.playerSizeY)) / 3);
        }
        protected void drawStartAndEnd(ICanvas canvas)
        {
            if (canvas is null)
            {
                throw new CanvasNotAvailableExpectation("");
            }
            canvas.StrokeColor = Colors.Blue;
            canvas.DrawCircle(maze.end.X+(float)(cellWidth/2), maze.end.Y+(float)(cellHeight/2), (float)Math.Min(cellWidth, cellHeight) / 3);
            canvas.StrokeColor = Colors.Red;
            canvas.DrawCircle(maze.start.X, maze.start.Y, (float)Math.Min(cellWidth, cellHeight) / 3);
            canvas.StrokeColor = Colors.Green;
        }
        protected void drawHitbox(ICanvas canvas)
        {
            if (canvas is null)
            {
                throw new CanvasNotAvailableExpectation("");
            }
            canvas.StrokeColor = Colors.Magenta;
            canvas.StrokeSize = 2;
            float minsize = MathF.Min((float)player.playerSizeX, ((float)player.playerSizeY));
            canvas.DrawRectangle(player.positionX + (float)player.playerSizeX - (minsize / 1.5f) / 2, player.positionY + (float)player.playerSizeY - (minsize / 1.5f) / 2, minsize / 1.5f, minsize / 1.5f);
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

                        if (walls[currX+1,currY] )
                        {
                            collisionX = true;
                        }
                        if (walls[currX , currY+1])
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
