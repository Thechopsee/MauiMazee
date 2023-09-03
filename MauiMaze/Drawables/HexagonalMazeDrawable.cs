using MauiMaze.Engine;
using MauiMaze.Models;


using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMaze.Drawables
{
    public class HexagonalMazeDrawable : IDrawable
    {
        /* private HoneyCombMaze maze;
         private Player player;
         public End end { get; set; }

         public double cellWidth { get; set; }
         public double cellHeight { get; set; }
         public Player GetPlayer()
         {
             return player;
         }
         public void setPlayer(Player player)
         {
             this.player = player;
         }

         private void DrawPolygon(ICanvas canvas, PointF[] points)
         {
             for (int i = 0; i < points.Length - 1; i++)
             {
                 var startPoint = points[i];
                 var endPoint = points[i + 1];
                 canvas.DrawLine(startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);
             }

             // Connect the last point with the first point to close the polygon
             var lastPoint = points[points.Length - 1];
             var firstPoint = points[0];
             canvas.DrawLine(lastPoint.X, lastPoint.Y, firstPoint.X, firstPoint.Y);
         }

         public void Draw(ICanvas canvas, RectF dirtyRect)
         {
             maze = new HoneyCombMaze(10);
             Application.Current.MainPage.DisplayAlert("Upozornění", "idn " + maze, "OK");
             // Calculate the size of a hexagon cell based on the maze size and canvas size
             cellWidth = dirtyRect.Width / 10;
             cellHeight = dirtyRect.Height / 10;

             // Iterate through the cells of the maze and draw hexagons
             for (int row = 0; row < 10; row++)
             {
                 for (int col = 0; col < 10; col++)
                 {
                     float x = col * (float)cellWidth;
                     float y = row * (float)cellHeight;

                     if (row % 2 == 1)
                     {
                         x += (float)cellWidth / 2; // Offset for odd rows
                     }

                     // Define the points for drawing the hexagon
                     PointF[] points = CalculateHexagonPoints(x, y);

                     // Draw the hexagon outline
                     canvas.StrokeColor = Colors.Black;
                     canvas.StrokeSize = 2;
                     DrawPolygon(canvas,points);

                 }
             }


         }

         private PointF[] CalculateHexagonPoints(float x, float y)
         {
             float halfWidth = (float)cellWidth / 2;
             float halfHeight = (float)cellHeight / 2;

             return new PointF[]
             {
                 new PointF(x + halfWidth, y),
                 new PointF(x + (float) cellWidth, y + halfHeight),
                 new PointF(x + halfWidth, y + (float) cellHeight),
                 new PointF(x, y + halfHeight),
                 new PointF(x, y),
                 new PointF(x + halfWidth, y - halfHeight)
             };
         }
        */
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            throw new NotImplementedException();
        }
    }

}
