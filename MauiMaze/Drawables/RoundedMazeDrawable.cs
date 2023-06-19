using MauiMaze.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMaze.Drawables
{
    public class RoundedMazeDrawable : IDrawable
    {
        private Maze maze;
        public Maze GetMaze(Maze maze)
        {
            return maze;
        }
        public void setNewMaze(Maze maze)
        {
            this.maze = maze;
        }
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeColor = Colors.Green;
            canvas.StrokeSize = 4;

            Maze maze = new Maze(new Size(25, 25),true);
            float centerX = dirtyRect.Center.X;
            float centerY = dirtyRect.Center.Y;
            float radius = Math.Min(dirtyRect.Width, dirtyRect.Height) / 2;

            foreach (var edge in maze.Edges)
            {
                float angle1 = (float)(edge.Cell1 * 2 * Math.PI / maze.Size.Width);
                float angle2 = (float)(edge.Cell2 * 2 * Math.PI / maze.Size.Width);

                float x1 = centerX + (float)(Math.Cos(angle1) * radius);
                float y1 = centerY + (float)(Math.Sin(angle1) * radius);
                float x2 = centerX + (float)(Math.Cos(angle2) * radius);
                float y2 = centerY + (float)(Math.Sin(angle2) * radius);

                canvas.DrawLine(x1, y1, x2, y2);
            }
        }

    }
}
