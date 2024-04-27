using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using MauiMaze.Models;
using System.Drawing;
using MauiMaze.Models.RoundedMaze;
using MauiMaze.Engine;
using MauiMaze.Exceptions;
using Microsoft.Maui.Controls.Compatibility;
using MauiMaze.Models.ClassicMaze;
using MauiMaze.Helpers;

namespace MauiMaze.Drawables
{
    public class RoundedMazeDrawable : BaseMazeDrawable,IDrawable
    {
        public RoundedMazeDrawable(GeneratorEnum? ge)
        {
            GeneratorEnum generator = ge ?? GeneratorEnum.Sets;
            if (maze is null)
            {
                maze = new RoundedMaze(new Microsoft.Maui.Graphics.Size(10, 10),generator);
            }
        }
        public RoundedMazeDrawable() { maze = new RoundedMaze(new Microsoft.Maui.Graphics.Size(10, 10), GeneratorEnum.Sets); }


    public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (canvas is null)
            {
                throw new CanvasNotAvailableExpectation("");
            }
            canvas.StrokeColor = Colors.Black;
            canvas.StrokeSize = 4;
            float left = dirtyRect.Left;
            float top = dirtyRect.Top;
            float right = dirtyRect.Right;
            float bottom = dirtyRect.Bottom;
            walls = new bool[(int)dirtyRect.Width, (int)dirtyRect.Height];

            canvas.DrawRectangle(left, top, right, bottom);

            RoundedMaze rm = (RoundedMaze)maze;
            if (rm.end is null)
            {
                rm.Generate(dirtyRect.Width, dirtyRect.Height);
            }
            canvas.StrokeColor = Colors.Blue;
            canvas.DrawCircle((float)rm.end.X, (float)rm.end.Y, 10);
            //canvas.DrawRectangle((float)rm.end.X, rm.end.Y, rm.end.bottomX - rm.end.X, rm.end.bottomY - rm.end.Y);
            canvas.StrokeColor = Colors.Red;
            canvas.DrawCircle((float)rm.start.X, (float)rm.start.Y , 10);
            canvas.StrokeColor = Colors.Green;
            foreach (var row in rm.grid)
            {
                foreach (var cell in row)
                {

                    if (cell.Row < 2)
                    {
                        continue;
                    }
                    float startX = cell.InnerCcwX;
                    float startY = cell.InnerCcwY;

                    if (cell.Inward == null || RoundedMaze.IsLinked(cell, cell.Inward))
                    {
                        canvas.DrawLine(startX, startY, cell.InnerCwX, cell.InnerCwY);
                    }

                    if (cell.Cw == null || RoundedMaze.IsLinked(cell, cell.Cw))
                    {
                        canvas.DrawLine(cell.InnerCwX, cell.InnerCwY, cell.OuterCwX, cell.OuterCwY);
                    }

                    if (cell.Row == rm.grid.Count - 1 && cell.Col != row.Count * 0.75)
                    {
                        canvas.DrawLine(cell.OuterCcwX, cell.OuterCcwY, cell.OuterCwX, cell.OuterCwY);
                    }
                }
            }

            if (player is null ||player.positionX<=0)
            {
               cellHeight = 20;
               cellWidth = 20;
               player = new Player((int)(maze.start.X-cellWidth), (int)(maze.start.Y-cellHeight), cellWidth, cellHeight);
            }
            else
            {
                drawPlayer(canvas);
            }

        }

    }
}
