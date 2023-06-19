using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiMaze.Models;
using Microsoft.Maui.Graphics;

namespace MauiMaze.Drawables
{
    public class MazeDrawable : IDrawable
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

            float left = dirtyRect.Left;
            float top = dirtyRect.Top;
            float right = dirtyRect.Right;
            float bottom = dirtyRect.Bottom;
            Console.WriteLine(right + " " + bottom);
            canvas.DrawRectangle(left, top, right, bottom);

            Maze maze = new Maze(new Size(10, 10));

            var cellWidth = dirtyRect.Width / maze.Size.Width;
            var cellHeight = dirtyRect.Height / maze.Size.Height;

            var cellSize = Math.Min(dirtyRect.Width / maze.Size.Width, dirtyRect.Height / maze.Size.Height);

            foreach (var edge in maze.Edges)
            {
                if (Math.Abs(edge.Cell1 - edge.Cell2) > 1)
                {
                    // Draw a horizontal line
                    float x = (float)(Math.Max(edge.Cell1, edge.Cell2) % maze.Size.Width * cellWidth);
                    float y = (float)(Math.Floor((double)Math.Min(edge.Cell1, edge.Cell2) / maze.Size.Width + 1) *
                            cellHeight);
                    canvas.DrawLine(x, y, (float)(x + cellWidth), y);
                }
                else
                {
                    // Draw a vertical line
                    float x = (float)(Math.Max(edge.Cell1, edge.Cell2) % maze.Size.Width * cellWidth);
                    float y = (float)(Math.Floor((double)Math.Min(edge.Cell1, edge.Cell2) / maze.Size.Width) * cellHeight);
                    canvas.DrawLine(x, y, x, y + (float)cellHeight);
                }
            }
            // Vykreslení červeného kolečka na začatku
            float startX = (float)(maze.Start % maze.Size.Width * cellWidth + cellWidth / 2);
            float startY = (float)(Math.Floor((double)maze.Start / maze.Size.Width) * cellHeight + cellHeight / 2);
            canvas.StrokeColor = Colors.Red;
            canvas.DrawCircle(startX, startY, (float)Math.Min(cellWidth, cellHeight) / 3);

            // Vykreslení modrého kolečka na konci
            float endX = (float)(maze.End % maze.Size.Width * cellWidth + cellWidth / 2);
            float endY = (float)(Math.Floor((double)maze.End / maze.Size.Width) * cellHeight + cellHeight / 2);
            canvas.StrokeColor = Colors.Blue;
            canvas.DrawCircle(endX, endY, (float)Math.Min(cellWidth, cellHeight) / 3);
            if (maze.path is not null)
            {
                // Vykreslení cesty
                if (maze.path != null)
                {
                    Application.Current.MainPage.DisplayAlert("Upozornění", "idn " + maze.path.Count + " " + maze.path, "OK");
                    canvas.StrokeColor = Colors.Yellow;
                    canvas.StrokeSize = 6;
                    for (int i = 0; i < maze.path.Count - 1; i++)
                    {
                        int cell1 = maze.path[i];
                        int cell2 = maze.path[i + 1];

                        float x1 = (float)(cell1 % maze.Size.Width * cellWidth + cellWidth / 2);
                        float y1 = (float)(Math.Floor((double)cell1 / maze.Size.Width) * cellHeight + cellHeight / 2);
                        float x2 = (float)(cell2 % maze.Size.Width * cellWidth + cellWidth / 2);
                        float y2 = (float)(Math.Floor((double)cell2 / maze.Size.Width) * cellHeight + cellHeight / 2);

                        canvas.DrawLine(x1, y1, x2, y2);
                    }
                }
            }

        }
       


    }   
}
