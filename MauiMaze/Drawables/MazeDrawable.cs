using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiMaze.Engine;
using MauiMaze.Models.ClassicMaze;
using Microsoft.Maui.Graphics;

using System.Reflection;
using IImage = Microsoft.Maui.Graphics.IImage;
#if IOS || ANDROID || MACCATALYST
using Microsoft.Maui.Graphics.Platform;
#elif WINDOWS
using Microsoft.Maui.Graphics.Win2D;
#endif


namespace MauiMaze.Drawables
{
    public class MazeDrawable : BaseMazeDrawable,IDrawable
    {

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeColor = Colors.Green;
            canvas.StrokeSize = 4;
            float left = dirtyRect.Left;
            float top = dirtyRect.Top;
            float right = dirtyRect.Right;
            float bottom = dirtyRect.Bottom;
            canvas.DrawRectangle(left, top, right, bottom);

            this.cellWidth = dirtyRect.Width / this.maze.Size.Width;
            this.cellHeight = dirtyRect.Height / this.maze.Size.Height;
            walls = new bool[(int)dirtyRect.Width, (int)dirtyRect.Height];
            Maze maze = (Maze)this.maze;
            int Start = 0;
            int End = maze.Edges.Length - 1;
            foreach (var edge in maze.Edges)
            {
                if (Math.Abs(edge.Cell1 - edge.Cell2) > 1)
                {
                    float x = (float)(Math.Max(edge.Cell1, edge.Cell2) % maze.Size.Width * cellWidth);
                    float y = (float)(Math.Floor((double)Math.Min(edge.Cell1, edge.Cell2) / maze.Size.Width + 1) *
                            cellHeight);
                    canvas.DrawLine(x, y, (float)(x + cellWidth), y);
                    double movefor = x+cellWidth;
                    if (movefor > dirtyRect.Width)
                    {
                        movefor = dirtyRect.Width;
                    }
                    for (int i = (int)x; i < movefor-1; i++)
                    {
                        walls[i, (int)y] = true;
                    }
                   
                }
                else
                {
                    float x = (float)(Math.Max(edge.Cell1, edge.Cell2) % maze.Size.Width * cellWidth);
                    float y = (float)(Math.Floor((double)Math.Min(edge.Cell1, edge.Cell2) / maze.Size.Width) * cellHeight);
                    canvas.DrawLine(x, y, x, y + (float)cellHeight);
                    double movefor = y + cellHeight;
                    if (movefor > dirtyRect.Height)
                    {
                        movefor = dirtyRect.Height;
                    }
                    for (int i = (int)y+2; i < movefor-1; i++)
                    {
                        walls[(int)x, i] = true;
                    }
                }
            }
            if (player is not null)
            {
                float startX = (float)(Start % maze.Size.Width * cellWidth + cellWidth / 2);
                float startY = (float)(Math.Floor((double)Start / maze.Size.Width) * cellHeight + cellHeight / 2);
                maze.start = new Start((int)startX, (int)startY);

                float endX = (float)(End % maze.Size.Width * cellWidth + cellWidth / 2);
                float endY = (float)(Math.Floor((double)End / maze.Size.Width) * cellHeight + cellHeight / 2);
                maze.end = new End((int)endX,(int)endY,(int)endX-((int)cellWidth/2),(int)endY-((int)cellHeight/2));
                drawStartAndEnd(canvas);

                drawPlayer(canvas);                

                 canvas.StrokeColor = Colors.Magenta; 
                 canvas.StrokeSize = 2;
                 float minsize = MathF.Min((float)player.playerSizeX, ((float)player.playerSizeY));
                 //player.recalculateHitbox();
                 canvas.DrawRectangle(player.positionX+(float)player.playerSizeX-(minsize / 1.5f) /2, player.positionY+ (float)player.playerSizeY-(minsize / 1.5f) /2,minsize/1.5f ,minsize/1.5f);
                 inicialized = true;
            }

        }
       


    }   
}
