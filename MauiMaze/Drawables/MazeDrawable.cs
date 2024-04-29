using MauiMaze.Engine;
using MauiMaze.Models.ClassicMaze;
using MauiMaze.Helpers;

namespace MauiMaze.Drawables
{
    public class MazeDrawable : BaseMazeDrawable,IDrawable
    {
        public void initStartEnd(int Start,int End)
        {
            float startX = (float)(Start % maze.Width * cellWidth + cellWidth / 2);
            float startY = (float)(Math.Floor((double)Start / maze.Width) * cellHeight + cellHeight / 2);
            maze.start = new Start((int)startX, (int)startY, Start);

            float endX = (float)(End % maze.Width * cellWidth + cellWidth);
            float endY = (float)(Math.Floor((double)End / maze.Width) * cellHeight);
            maze.end = new End((int)endX, (int)endY, (int)endX + ((int)cellWidth), (int)endY + ((int)cellHeight), End);
        }
        public override void drawWalls(ICanvas canvas, RectF dirtyRect)
        {
            Maze maze = (Maze)this.maze;
            this.mazeWidth = dirtyRect.Width;
            this.mazeHeight = dirtyRect.Height;
            this.cellWidth = dirtyRect.Width / this.maze.Width;
            this.cellHeight = dirtyRect.Height / this.maze.Height;
            canvas.StrokeColor = Colors.Green;
            canvas.StrokeSize = 4;
            canvas.DrawRectangle(dirtyRect.Left, dirtyRect.Top, dirtyRect.Right, dirtyRect.Bottom);

            foreach (var edge in maze.Edges)
            {
                if (Math.Abs(edge.Cell1 - edge.Cell2) > 1)
                {
                    float x = (float)(Math.Max(edge.Cell1, edge.Cell2) % maze.Width * cellWidth);
                    float y = (float)(Math.Floor((double)Math.Min(edge.Cell1, edge.Cell2) / maze.Width + 1) * cellHeight);
                    canvas.DrawLine(x, y, (float)(x + cellWidth), y);
                    double movefor = x + cellWidth;
                    if (movefor > dirtyRect.Width)
                    {
                        movefor = dirtyRect.Width;
                    }
                    if (player is not null )
                    {
                        if (!player.dummy)
                        {
                            for (int i = (int)x; i < movefor-1; i++)
                            {
                                walls[i, (int)y] = true;
                                if (walls.GetLength(1) < y+1 || walls.GetLength(1) == y+2 )
                                {
                                    walls[i, (int)y - 1] = true;
                                    walls[i, (int)y - 2] = true;
                                }
                                else
                                {
                                walls[i, (int)y+1] = true;
                                walls[i, (int)y+2] = true;
                                }
                            }
                        }
                    }

                }
                else
                {
                    float x = (float)(Math.Max(edge.Cell1, edge.Cell2) % maze.Width * cellWidth);
                    float y = (float)(Math.Floor((double)Math.Min(edge.Cell1, edge.Cell2) / maze.Width) * cellHeight);
                    canvas.DrawLine(x, y, x, y + (float)cellHeight);
                    double movefor = y + cellHeight;
                    if (movefor > dirtyRect.Height)
                    {
                        movefor = dirtyRect.Height;
                    }
                    if (player is not null )
                    {
                        if (!player.dummy)
                        {
                            for (int i = (int)y; i < movefor-1; i++)
                            {
                                walls[(int)x, i] = true;
                                if (walls.GetLength(0) < x + 1 || walls.GetLength(0) == x + 2)
                                {
                                    walls[(int)x-1, i] = true;
                                    walls[(int)x-2, i] = true;
                                }
                                else
                                {
                                    walls[(int)x + 1, i] = true;
                                    walls[(int)x + 2, i] = true;
                                }
                            }
                        }
                    }
                }
                if (maze.start is not null)
                {
                    if (maze.end.X == -1)
                    {

                        initStartEnd(maze.start.cell,maze.end.cell);
                    }
                }
            }
        }
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeColor = Colors.Green;
            canvas.StrokeSize = 4;
            canvas.DrawRectangle(dirtyRect.Left, dirtyRect.Top, dirtyRect.Right, dirtyRect.Bottom);

            walls = new bool[(int)dirtyRect.Width, (int)dirtyRect.Height];
            drawWalls(canvas,dirtyRect);
            if (player is not null )
            {
                if (!player.dummy)
                {
                    if (maze.start is null || maze.end is null)
                    {
                        int Start = 0;
                        int dolniHranice = (int)((maze.Width * maze.Height) * 0.75);
                        int horniHranice = maze.Width * maze.Height;
                        Random random = new Random();
                        int End = random.Next(dolniHranice, horniHranice - 1);
                        initStartEnd(Start, End);
                    }
                    else if (maze.end.X == -1)
                    {
                        initStartEnd(maze.start.cell, maze.end.cell);
                    }
                }
                drawStartAndEnd(canvas);
                drawPlayer(canvas);
                drawHitbox(canvas);
            }
            drawStartAndEnd(canvas);
        }
    }   
}
