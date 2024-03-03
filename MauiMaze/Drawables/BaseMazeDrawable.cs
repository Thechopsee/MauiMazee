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
        public bool[,] walls { get; set; }


        public virtual void drawWalls(ICanvas canvas, RectF dirtyRect) { }

        public void reinitPlayer(Player player)
        {
            if (player is null) { player = new Player(0,0,32,32); };
            if (maze.start is null)
            {
                return;
            }
            player.reInit((int)maze.start.X, (int)maze.start.Y ,cellWidth,cellHeight);
            
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
        public void drawStartAndEnd(ICanvas canvas)
        {
            if (canvas is null)
            {
                throw new CanvasNotAvailableExpectation("");
            }
            if (maze.end is not null || maze.start is not null)
            {
                canvas.StrokeColor = Colors.Blue;
                canvas.DrawCircle(maze.end.X + (float)(cellWidth / 2), maze.end.Y + (float)(cellHeight / 2), (float)Math.Min(cellWidth, cellHeight) / 3);
                canvas.StrokeColor = Colors.Red;
                canvas.DrawCircle(maze.start.X, maze.start.Y, (float)Math.Min(cellWidth, cellHeight) / 3);
                canvas.StrokeColor = Colors.Green;
            }
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
        
    }
}
