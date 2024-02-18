using MauiMaze.Engine;
using MauiMaze.Helpers;
using MauiMaze.Models.ClassicMaze;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SQLite.TableMapping;

namespace MauiMaze.Drawables
{
    
    public class HeatmapDrawable : IDrawable
    {
        Maze maze { get; set; }
        public HeatmapDrawable(Maze maze, CellData[] cellData) { 
            this.maze = maze;
            this.cellData = cellData;
        }
        public CellData[] cellData { get; set; } 
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeColor = Colors.Black;
            canvas.StrokeSize = 4;
            
            double cellWidth = dirtyRect.Width / this.maze.Width;
            double cellHeight = dirtyRect.Height / this.maze.Height;
            

            Maze maze = this.maze;

            foreach (var cell in cellData)
            {
                int column = cell.num % maze.Width;
                int row = cell.num / maze.Width;
                canvas.FillColor = cell.color;

                canvas.FillRectangle((float)((column *cellWidth)), (float)(row*cellHeight), (float)(cellWidth), (float)(cellHeight));
                canvas.FontColor = Colors.Black;
                canvas.FontSize = 10;
                canvas.Font = Microsoft.Maui.Graphics.Font.Default;
                canvas.DrawString(""+cell.time, (float)((column * cellWidth)), (float)(row * cellHeight), (float)(cellWidth), (float)(cellHeight), HorizontalAlignment.Center, VerticalAlignment.Top);
                canvas.DrawString("" + cell.hit, (float)((column * cellWidth)+10), (float)(row * cellHeight+10), (float)(cellWidth)-10, (float)(cellHeight)-10, HorizontalAlignment.Center, VerticalAlignment.Center);
                //canvas.DrawText(""+cell.time, (float)((column * cellWidth)), (float)(row * cellHeight));
            }
            foreach (var edge in maze.Edges)
            {
                float x;
                float y;
                if (Math.Abs(edge.Cell1 - edge.Cell2) > 1)
                {
                    x = (float)(Math.Max(edge.Cell1, edge.Cell2) % maze.Width * cellWidth);
                    y = (float)(Math.Floor((double)Math.Min(edge.Cell1, edge.Cell2) / maze.Width + 1) * cellHeight);
                    canvas.DrawLine(x, y, (float)(x + cellWidth), y);
                }
                else
                {
                    x = (float)(Math.Max(edge.Cell1, edge.Cell2) % maze.Width * cellWidth);
                    y = (float)(Math.Floor((double)Math.Min(edge.Cell1, edge.Cell2) / maze.Width) * cellHeight);
                    canvas.DrawLine(x, y, x, y + (float)cellHeight);
                }
            }

            if (maze.end is not null || maze.start is not null)
            {
                canvas.StrokeColor = Colors.Blue;
                canvas.DrawCircle(maze.end.X + (float)(cellWidth / 2), maze.end.Y + (float)(cellHeight / 2), (float)Math.Min(cellWidth, cellHeight) / 3);
                canvas.StrokeColor = Colors.Red;
                canvas.DrawCircle(maze.start.X, maze.start.Y, (float)Math.Min(cellWidth, cellHeight) / 3);
                canvas.StrokeColor = Colors.Green;
                int column = cellData[maze.start.cell].num % maze.Width;
                int row = cellData[maze.start.cell].num / maze.Width;
                canvas.DrawString("" + cellData[maze.start.cell].time, (float)((column * cellWidth)), (float)(row * cellHeight), (float)(cellWidth), (float)(cellHeight), HorizontalAlignment.Center, VerticalAlignment.Top);

                column = cellData[maze.end.cell].num % maze.Width;
                row = cellData[maze.end.cell].num / maze.Width;
                canvas.DrawString("" + cellData[maze.end.cell].time, (float)((column * cellWidth)), (float)(row * cellHeight), (float)(cellWidth), (float)(cellHeight), HorizontalAlignment.Center, VerticalAlignment.Top);
            }
            canvas.DrawRectangle(dirtyRect.Left, dirtyRect.Top, dirtyRect.Right, dirtyRect.Bottom);
        }
    }
}
