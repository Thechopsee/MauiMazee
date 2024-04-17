using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiMaze.Models.ClassicMaze;
using MauiMaze.Engine;
using MauiMaze.Helpers;

namespace MauiMaze.Drawables
{
    public class MoveVizualizerDrawable : IDrawable
    {
        public List<GameRecord> gameRecords { get; set; }
        public BaseMazeDrawable mazeDrawable { get; set; }
        public bool showCell { get; set; }
        public bool showAll { get; set; }
        public int actualID { get; set; }

        public void sendMaze(GameMaze maze)
        {
            if (maze is Maze)
            {
                mazeDrawable = new MazeDrawable();
            }
            else
            {
                mazeDrawable = new RoundedMazeDrawable(null);
            }
            mazeDrawable.maze = maze;
        }

        public static (int, int) GetPlayerPositionInMazeSize(double xPercentage, double yPercentage, int currentMazeWidth, int currentMazeHeight)
        {

            int newX = (int)(xPercentage * currentMazeWidth);
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
                    int y = gr.cellPath[i] / (int)mazeDrawable.maze.Height;
                    int x = gr.cellPath[i] % (int)mazeDrawable.maze.Height;
                    double posx = x * mazeDrawable.cellWidth + (mazeDrawable.cellWidth / 2);
                    double posy = y * mazeDrawable.cellHeight + (mazeDrawable.cellHeight / 2);
                    int y2 = gr.cellPath[i + 1] / (int)mazeDrawable.maze.Height;
                    int x2 = gr.cellPath[i + 1] % (int)mazeDrawable.maze.Height;
                    double posx2 = x2 * mazeDrawable.cellWidth + (mazeDrawable.cellWidth / 2);
                    double posy2 = y2 * mazeDrawable.cellHeight + (mazeDrawable.cellHeight / 2);
                    canvas.DrawLine((float)posx, (float)posy, (float)(posx2), (float)(posy2));
                }
            }
        }

        public void  Draw(ICanvas canvas, RectF dirtyRect)
        {
            mazeDrawable.drawWalls(canvas, dirtyRect);
            mazeDrawable.drawStartAndEnd(canvas);

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
                if (mazeDrawable.player is not null)
                {
                    mazeDrawable.drawPlayer(canvas);
                }
            }
        }
    }
}
