﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using MauiMaze.Models;
using System.Drawing;
using MauiMaze.Models.RoundedMaze;
using MauiMaze.Engine;

namespace MauiMaze.Drawables
{
    public class RoundedMazeDrawable : BaseMazeDrawable,IDrawable
    {
        public RoundedMazeDrawable()
        {
            if (maze is null)
            {
                maze = new RoundedMaze(new Microsoft.Maui.Graphics.Size(25,25));
            }
        }


         
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            
            canvas.StrokeColor = Colors.Black;
            canvas.StrokeSize = 4;
            float left = dirtyRect.Left;
            float top = dirtyRect.Top;
            float right = dirtyRect.Right;
            float bottom = dirtyRect.Bottom;
            walls = new bool[(int)dirtyRect.Width, (int)dirtyRect.Height];

            canvas.DrawRectangle(left, top, right, bottom);
            if (!maze.firstrun)
            {
                maze.generateProcedure((int)dirtyRect.Height, (int)dirtyRect.Width);
                maze.SolveAndDraw(canvas);
            }
            else {
                maze.JustDraw(canvas);
            }
            cellHeight = 80;
            cellWidth = 80;

            if (player is null)
            {
                cellHeight = 80;
                cellWidth = 80;
                player = new Player((int)maze.start.X, (int)maze.start.Y, cellWidth, cellHeight);
            }
            else
            {
                drawPlayer(canvas);
            }
        }

    }
}
