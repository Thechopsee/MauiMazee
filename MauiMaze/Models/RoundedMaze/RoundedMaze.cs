
using MauiMaze.Engine;
using MauiMaze.Exceptions;
using MauiMaze.Helpers;
using MauiMaze.Models.ClassicMaze;
using MauiMaze.Models.Generatory;
using MauiMaze.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMaze.Models.RoundedMaze
{

    public class RoundedMaze : GameMaze
    {
        public List<List<Cell>> grid = new List<List<Cell>>();
        private int size = 25;
        private int width = 0;
        private int rows = 0;
        private int cols = 0;
        public int xoffsett { get; set; }
        public int yoffsett { get; set; }

        public RoundedMaze(Size size,GeneratorEnum ge)
        {
            Width = (int)size.Width;
            Height = (int)size.Height;
            mazeType = MazeType.Rounded;
            this.size = (int)size.Height+15;
            if (ge == GeneratorEnum.Sets)
            {
                Edges = new SetsGenerator().GenerateMaze((int)size.Width, (int)size.Height).ToArray();
            }
            else
            {
                Edges = new HuntAndKillMazeGenerator().GenerateMaze((int)size.Height, (int)size.Width).ToArray();
            }
        }
        public RoundedMaze(Size size, Edge[] edges)
        {
            Width = (int)size.Width;
            Height = (int)size.Height;
            mazeType = MazeType.Rounded; 
            this.size = (int)size.Height + 15;
            this.Edges = edges;
            rows = (int)Width;
            cols = (int)Height;
            createGrid();
            recostructFromEdges();
        }

        void createGrid()
        { 
            grid = new List<List<Cell>>();
            grid.Add(new List<Cell> { new Cell { Row = 0, Col = 0, Links = new List<Link>() } });

            for (int i = 1; i < rows; i++)
            {
                List<Cell> rowList = new List<Cell>();

                for (int j = 0; j < cols; j++)
                {
                    rowList.Add(new Cell { Row = i, Col = j, Links = new List<Link>() });
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

                        double ratio = (double)grid[i].Count / grid[i - 1].Count;
                        Cell parent = grid[i - 1][(int)Math.Floor(j / ratio)];

                        cell.Inward = new Cell { Row = parent.Row, Col = parent.Col };
                    }
                }
            }

            PositionCells();
        }
        public void PositionCells()
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

                    cell.InnerCcwX = (int)(Math.Round(center + (innerRadius * Math.Cos(angleCcw)))  )+xoffsett;
                    cell.InnerCcwY = (int)(Math.Round(center + (innerRadius * Math.Sin(angleCcw)))  )+yoffsett;
                    cell.OuterCcwX = (int)(Math.Round(center + (outerRadius * Math.Cos(angleCcw))) ) +xoffsett;
                    cell.OuterCcwY = (int)(Math.Round(center + (outerRadius * Math.Sin(angleCcw))) ) +yoffsett;
                    cell.InnerCwX = (int)(Math.Round(center + (innerRadius * Math.Cos(angleCw))) ) + xoffsett;
                    cell.InnerCwY = (int)(Math.Round(center + (innerRadius * Math.Sin(angleCw))) ) + yoffsett;
                    cell.OuterCwX = (int)(Math.Round(center + (outerRadius * Math.Cos(angleCw))) ) + xoffsett;
                    cell.OuterCwY = (int)(Math.Round(center + (outerRadius * Math.Sin(angleCw))) ) + yoffsett;

                    double centerAngle = (angleCcw + angleCw) / 2.0;

                    cell.CenterX = (int)((Math.Round(center + (innerRadius * Math.Cos(centerAngle)))  +
                                       Math.Round(center + (outerRadius * Math.Cos(centerAngle)))) / 2);
                    cell.CenterY = (int)((Math.Round(center + (innerRadius * Math.Sin(centerAngle)))  +
                                       Math.Round(center + (outerRadius * Math.Sin(centerAngle))) ) / 2);
                    
                }
            }
            
            
        }
        public static bool IsLinked(Cell cellA, Cell cellB)
        {
            return cellA.Links.Any(link => link.Row == cellB.Row && link.Col == cellB.Col);
        }

        private void recostructFromEdges()
        {
            for (int i = 0; i < Edges.Length; i++)
            {
                int rowc1 = (int)(Edges[i].Cell1 / Width);
                int colc1 = (int)(Edges[i].Cell1 % Width);

                int rowc2 = (int)(Edges[i].Cell2 / Width);
                int colc2 = (int)(Edges[i].Cell2 % Width);

                if (rowc1 < 2 || rowc2 < 2)
                {
                    continue;
                }
                grid[rowc1][colc1].Links.Add(new Link { Row = rowc2, Col = colc2 });
                grid[rowc2][colc2].Links.Add(new Link { Row = rowc1, Col = colc1 });
            }
        }
        public void Generate(double width, double height)
        {
            rows = (int)Width;
            cols = (int)Height;
            this.width = (int)height;
            xoffsett = ((int)width / 2) - this.width / 2;
            yoffsett = 0;
            end = new End((this.width / 2) + xoffsett, (this.width / 2) + yoffsett, (this.width / 2) + xoffsett + 30, (this.width / 2) + yoffsett + 30, 0);
            start = new Start((this.width / 2)+xoffsett , (this.width / 14) ,1);
            createGrid();
            recostructFromEdges();
        }
       
    }
}
