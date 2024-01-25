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
using static Plugin.LocalNotification.NotificationRequestGeofence;
using MauiMaze.Helpers;
#if IOS || ANDROID || MACCATALYST
using Microsoft.Maui.Graphics.Platform;
#elif WINDOWS
using Microsoft.Maui.Graphics.Win2D;
#endif


namespace MauiMaze.Drawables
{
    public class MazeDrawable : BaseMazeDrawable,IDrawable
    {
        
        public static (int, int) GetPlayerPositionInMazeSize(double xPercentage,double yPercentage, int currentMazeWidth, int currentMazeHeight)
        {

            int newX = (int)(xPercentage* currentMazeWidth);
            int newY = (int)(yPercentage * currentMazeHeight);

            return (newX, newY);
        }

        public void drawMotion(ICanvas canvas, GameRecord gr, int count, RectF dirtyRect)
        {
            canvas.StrokeColor = ColorSchemeProvider.getColor(count);
            gr.color = ColorSchemeProvider.getColor(count);
            canvas.StrokeSize = 2;
            if (!showCell)
            {
                for (int i = 0; i < gr.moves.Count - 1; i++)
                {

                    (int, int) position = GetPlayerPositionInMazeSize(gr.moves[i].percentagex, gr.moves[i].percentagey, (int)dirtyRect.Width, (int)dirtyRect.Height);
                    (int, int) position1 = GetPlayerPositionInMazeSize(gr.moves[i + 1].percentagex, gr.moves[i + 1].percentagey, (int)dirtyRect.Width, (int)dirtyRect.Height);
                    canvas.DrawLine(position.Item1, position.Item2, position1.Item1, position1.Item2);
                }
            }
            else
            {
                for (int i = 0; i < gr.cellPath.Count - 1; i++)
                {
                    int y = gr.cellPath[i] / (int)maze.Size.Height;
                    int x = gr.cellPath[i] % (int)maze.Size.Height;
                    double posx = x * cellWidth+(cellWidth/2);
                    double posy = y * cellHeight+(cellHeight/2);
                    int y2 = gr.cellPath[i + 1] / (int)maze.Size.Height;
                    int x2 = gr.cellPath[i + 1] % (int)maze.Size.Height;
                    double posx2 = x2 * cellWidth+(cellWidth/2);
                    double posy2 = y2 * cellHeight+(cellHeight / 2);
                    canvas.DrawLine((float)posx, (float)posy, (float)(posx2), (float)(posy2));
                }
            }
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeColor = Colors.Green;
            canvas.StrokeSize = 4;
            canvas.DrawRectangle(dirtyRect.Left, dirtyRect.Top, dirtyRect.Right, dirtyRect.Bottom);
            this.mazeWidth = dirtyRect.Width;
            this.mazeHeight = dirtyRect.Height;
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
                    float y = (float)(Math.Floor((double)Math.Min(edge.Cell1, edge.Cell2) / maze.Size.Width + 1) * cellHeight);
                    canvas.DrawLine(x, y, (float)(x + cellWidth), y);
                    double movefor = x+cellWidth;
                    if (movefor > dirtyRect.Width)
                    {
                        movefor = dirtyRect.Width;
                    }
                    if (player is not null)
                    {
                        for (int i = (int)x; i < movefor - 1; i++)
                        {
                            walls[i, (int)y] = true;
                        }
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
                    if (player is not null)
                    {
                        for (int i = (int)y + 2; i < movefor - 1; i++)
                        {
                            walls[(int)x, i] = true;
                        }
                    }
                }
            }
            if (player is not null)
            {
                if (maze.start is null || maze.end is null)
                {
                    float startX = (float)(Start % maze.Size.Width * cellWidth + cellWidth / 2);
                    float startY = (float)(Math.Floor((double)Start / maze.Size.Width) * cellHeight + cellHeight / 2);
                    maze.start = new Start((int)startX, (int)startY);

                    float endX = (float)(End % maze.Size.Width * cellWidth + cellWidth);
                    float endY = (float)(Math.Floor((double)End / maze.Size.Width) * cellHeight + cellHeight);
                    maze.end = new End((int)endX, (int)endY, (int)endX + ((int)cellWidth), (int)endY + ((int)cellHeight));
                }
                drawStartAndEnd(canvas);
                drawPlayer(canvas);
                drawHitbox(canvas);
            }
            if (preview is not null)
            {
                drawPreview(canvas);    
            }
            if (gameRecords is not null)
            {
                int count = 0;
                if (showAll)
                {
                    foreach (GameRecord gr in gameRecords)
                    {
                        drawMotion(canvas, gr, count, dirtyRect);
                        count++;
                    }
                }
                else
                {
                    drawMotion(canvas, gameRecords.ElementAt(actualID), actualID, dirtyRect);
                }
            }
        }
    }   
}
