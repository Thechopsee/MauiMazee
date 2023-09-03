
using MauiMaze.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMaze.Models.RoundedMaze
{
    public class RoundedMaze :GameMaze
    {
        private List<List<Cell>> grid = new List<List<Cell>>();
        private int lineWidth = 4;
        private int size = 25;
        private int width = 0;
        private int rows = 0;
        private int xoffsett;
        private int yoffsett;

        public RoundedMaze(Size size)
        {
            this.size = (int)size.Height;
        }
        public override void generateProcedure(int height,int width)
        {
            Resize(false, height);
            xoffsett = (width /2)-this.width/2;
            yoffsett = 30;
            end = new End((this.width / 2) + xoffsett, this.width / 2, (this.width / 2) + xoffsett+10, this.width / 2+10);
            createGrid();
        }
        void createGrid()
        {
            double rowHeight = 1.0 / rows;

            grid = new List<List<Cell>>();
            grid.Add(new List<Cell> { new Cell { Row = 0, Col = 0, Links = new List<Link>(), Outward = new List<Link>() } });

            for (int i = 1; i < rows; i++)
            {
                double radius = (double)i / rows;
                double circumference = 2 * Math.PI * radius;
                int prevCount = grid[i - 1].Count;
                double cellWidth = circumference / prevCount;
                int ratio = (int)Math.Round(cellWidth / rowHeight);
                int count = prevCount * ratio;

                List<Cell> rowList = new List<Cell>();

                for (int j = 0; j < count; j++)
                {
                    rowList.Add(new Cell { Row = i, Col = j, Links = new List<Link>(), Outward = new List<Link>() });
                }

                grid.Add(rowList);
            }

            for (int i = 0; i < grid.Count; i++)
            {
                for (int j = 0; j < grid[i].Count; j++)
                {
                    Cell cell = grid[i][j];
                    if (cell.Row > 0)
                    {
                        cell.Cw = new Cell { Row = i, Col = (j == grid[i].Count - 1 ? 0 : j + 1) };
                        cell.Ccw = new Cell { Row = i, Col = (j == 0 ? grid[i].Count - 1 : j - 1) };

                        double ratio = (double)grid[i].Count / grid[i - 1].Count;
                        Cell parent = grid[i - 1][(int)Math.Floor(j / ratio)];

                        cell.Inward = new Cell { Row = parent.Row, Col = parent.Col };
                        parent.Outward.Add(new Link { Row = cell.Row, Col = cell.Col });
                    }
                }
            }

            PositionCells();
        }
        void PositionCells()
        {
            double center = width / 2.0;

            
            foreach (var row in grid)
            {
                foreach (var cell in row)
                {
                    double angle = 2 * Math.PI / row.Count;
                    double innerRadius = cell.Row * size;
                    double outerRadius = (cell.Row + 1) * size;
                    double angleCcw = cell.Col * angle;
                    double angleCw = (cell.Col + 1) * angle;

                    cell.InnerCcwX = (int)(Math.Round(center + (innerRadius * Math.Cos(angleCcw))) + lineWidth / 2)+xoffsett;
                    cell.InnerCcwY = (int)(Math.Round(center + (innerRadius * Math.Sin(angleCcw))) + lineWidth / 2)+yoffsett;
                    cell.OuterCcwX = (int)(Math.Round(center + (outerRadius * Math.Cos(angleCcw))) + lineWidth / 2) + xoffsett;
                    cell.OuterCcwY = (int)(Math.Round(center + (outerRadius * Math.Sin(angleCcw))) + lineWidth / 2) + yoffsett;
                    cell.InnerCwX = (int)(Math.Round(center + (innerRadius * Math.Cos(angleCw))) + lineWidth / 2) + xoffsett;
                    cell.InnerCwY = (int)(Math.Round(center + (innerRadius * Math.Sin(angleCw))) + lineWidth / 2) + yoffsett;
                    cell.OuterCwX = (int)(Math.Round(center + (outerRadius * Math.Cos(angleCw))) + lineWidth / 2) + xoffsett;
                    cell.OuterCwY = (int)(Math.Round(center + (outerRadius * Math.Sin(angleCw))) + lineWidth / 2) + yoffsett;

                    double centerAngle = (angleCcw + angleCw) / 2.0;

                    cell.CenterX = (int)((Math.Round(center + (innerRadius * Math.Cos(centerAngle))) + lineWidth / 2 +
                                       Math.Round(center + (outerRadius * Math.Cos(centerAngle))) + lineWidth / 2) / 2);
                    cell.CenterY = (int)((Math.Round(center + (innerRadius * Math.Sin(centerAngle))) + lineWidth / 2 +
                                       Math.Round(center + (outerRadius * Math.Sin(centerAngle))) + lineWidth / 2) / 2);
                    
                }
            }
            
            
        }
        private bool IsLinked(Cell cellA, Cell cellB)
        {
            return cellA.Links.Any(link => link.Row == cellB.Row && link.Col == cellB.Col);
        }

        private List<Cell> GetNeighbors(Cell cell)
        {
            var list = new List<Cell>();

            if (cell.Cw != null) list.Add(grid[cell.Cw.Row][cell.Cw.Col]);
            if (cell.Ccw != null) list.Add(grid[cell.Ccw.Row][cell.Ccw.Col]);
            if (cell.Inward != null) list.Add(grid[cell.Inward.Row][cell.Inward.Col]);

            list.AddRange(cell.Outward.Select(outward => grid[outward.Row][outward.Col]));

            return list;
        }

        public override void SolveAndDraw(ICanvas canvas)
        {
            Random random = new Random();
            int randomRow = random.Next(rows);
            int randomCol = random.Next(grid[randomRow].Count);

            Cell current = grid[randomRow][randomCol];
            Cell last = grid[grid.Count - 1][grid[grid.Count-1].Count- grid[grid.Count - 1].Count/4];
            
           while (current != null)
            {
                List<Cell> unvisitedNeighbors = GetNeighbors(current).Where(n => n.Links.Count == 0).ToList();
                int length = unvisitedNeighbors.Count;

                if (length > 0)
                {
                    int rand = random.Next(length);
                    Cell neighbor = unvisitedNeighbors[rand];

                    current.Links.Add(new Link { Row = neighbor.Row, Col = neighbor.Col });
                    grid[neighbor.Row][neighbor.Col].Links.Add(new Link { Row = current.Row, Col = current.Col });

                    current = neighbor;
                }
                else
                {
                    
                    current = null;

                    for (int i = 0; i < grid.Count; i++)
                    {
                        for (int j = 0; j < grid[i].Count; j++)
                        {
                            List<Cell> visitedNeighbors = GetNeighbors(grid[i][j]).Where(n => n.Links.Count > 0).ToList();

                            if (grid[i][j].Links.Count == 0 && visitedNeighbors.Count > 0)
                            {
                                current = grid[i][j];
                                int rand = random.Next(visitedNeighbors.Count);
                                Cell neighbor = visitedNeighbors[rand];

                                current.Links.Add(new Link { Row = neighbor.Row, Col = neighbor.Col });
                                grid[neighbor.Row][neighbor.Col].Links.Add(new Link { Row = current.Row, Col = current.Col });

                                break;
                            }
                        }

                        if (current != null)
                            break;
                    }
                }
            }
            if (start is null)
            {
                start = new Start((int)last.CenterX + xoffsett,(int)last.CenterY);
            }
            RenderMaze(canvas);
        }

        public override void JustDraw(ICanvas canvas) { RenderMaze(canvas); }
        private void RenderMaze(ICanvas canvas)
        {
            //Application.Current.MainPage.DisplayAlert("Upozornění", "run "+grid.Count, "OK");
            canvas.StrokeColor = Colors.Red;
            canvas.DrawCircle((float)end.X, (float)end.Y+yoffsett, 10);
            canvas.DrawCircle((float)start.X, (float)start.Y + yoffsett, 10);
            canvas.StrokeColor = Colors.Black;

            foreach (var row in grid)
            {
                foreach (var cell in row)
                {
                    float startX = cell.InnerCcwX;
                    float startY = cell.InnerCcwY;


                    if (cell.Inward == null || !IsLinked(cell, cell.Inward))
                    {
                        canvas.DrawLine(startX, startY, cell.InnerCwX, cell.InnerCwY);
                    }

                    if (cell.Cw == null || !IsLinked(cell, cell.Cw))
                    {
                        canvas.DrawLine(cell.InnerCwX, cell.InnerCwY, cell.OuterCwX, cell.OuterCwY);
                    }

                    if (cell.Row == grid.Count - 1 && cell.Col != row.Count * 0.75)
                    {
                        canvas.DrawLine(cell.OuterCcwX, cell.OuterCcwY, cell.OuterCwX, cell.OuterCwY);
                    }
                }
            }
        }

        void Resize(bool change, int height)
        {
            width = height; //TODO min

            if (change)
            {
                size = (int)Math.Floor(width / 2.0 / rows);
            }
            else
            {
                rows = (int)Math.Floor(width / 2.0 / size);
            }

            width = 2 * rows * size;

        }
    }
}
