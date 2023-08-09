
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMaze.Models
{
    public class Maze
    {
        public Edge[] Edges { get; }
        public Size Size { get; }
        public int Start { get; } // Počáteční bod
        public int End { get; } // Koncový bod
        public List<List<Edge>> PassableEdges { get; }

        public List<int> path { get;}

        public int[] getPositionsOfStart()
        {
            int[] positions = new int[2];
            positions[0] = Convert.ToInt32(Start / Convert.ToInt32(Size.Width));
            positions[1] = Convert.ToInt32(Start % Convert.ToInt32(Size.Height));
            return positions;
        }
        public bool isThereWall(int x1 ,int y1,int x2,int y2)
        {
            int currentCellIndex = Convert.ToInt32(y1 * Size.Width + x1);
            int nextCellIndex = Convert.ToInt32(y2 * Size.Width + x2);
            if (Edges.Contains(new Edge(currentCellIndex, nextCellIndex)) || Edges.Contains(new Edge(nextCellIndex, currentCellIndex)))
            {
                return true;
            }
            return false;
        }
        public Maze(Size size)
        {
            var edgesToCheck = Edge.Generate(size);
            var sets = new DisjointSets(Convert.ToInt32(size.Height * size.Width));
            var mazeEdges = new List<Edge>();

            var random = new Random();

            while (sets.Count > 1 && edgesToCheck.Count > 0)
            {
                // Pick a random edge
                var edgeIndex = random.Next(edgesToCheck.Count);
                var edge = edgesToCheck[edgeIndex];

                // Find the two sets separated by the edge
                var set1 = sets.Find(edge.Cell1);
                var set2 = sets.Find(edge.Cell2);

                if (set1 != set2)
                    sets.Union(set1, set2);
                else
                    mazeEdges.Add(edge);

                // Remove edge, so it won't be checked again
                edgesToCheck.RemoveAt(edgeIndex);
            }

            // Set maze properties
            Edges = edgesToCheck.Concat(mazeEdges).ToArray();
            Size = size;
            // Set start and end points

            // Set maze properties
            PassableEdges = new List<List<Edge>>();
            for (int row = 0; row < size.Height; row++)
            {
                var rowEdges = new List<Edge[]>();

                for (int column = 0; column < size.Width; column++)
                {
                    var cellEdges = new List<Edge>();

                    if (column > 0)
                    {
                        int leftCellIndex = Convert.ToInt32(row * size.Width + column - 1);
                        var edge1 = new Edge(Convert.ToInt32(row * size.Width + column), leftCellIndex);
                        var edge2 = new Edge(leftCellIndex, Convert.ToInt32(row * size.Width + column));
                        if (!Edges.Contains(edge1) && !Edges.Contains(edge2) && !rowEdges.Any(edges => edges.Contains(edge2)))
                        {
                            cellEdges.Add(edge1);
                        }
                    }
                    if (column < size.Width - 1)
                    {
                        int rightCellIndex = Convert.ToInt32(row * size.Width + column + 1);
                        var edge1 = new Edge(Convert.ToInt32(row * size.Width + column), rightCellIndex);
                        var edge2 = new Edge(rightCellIndex, Convert.ToInt32(row * size.Width + column));
                        if (!Edges.Contains(edge1) && !Edges.Contains(edge2) && !cellEdges.Contains(edge2) && !rowEdges.Any(edges => edges.Contains(edge2)))
                        {
                            cellEdges.Add(edge1);
                        }
                    }
                    if (row > 0)
                    {
                        int topCellIndex = Convert.ToInt32((row - 1) * size.Width + column);
                        var edge1 = new Edge(Convert.ToInt32(row * size.Width + column), topCellIndex);
                        var edge2 = new Edge(topCellIndex, Convert.ToInt32(row * size.Width + column));
                        if (!Edges.Contains(edge1) && !Edges.Contains(edge2) && !cellEdges.Contains(edge2) && !rowEdges.Any(edges => edges.Contains(edge2)))
                        {
                            cellEdges.Add(edge1);
                        }
                    }
                    if (row < size.Height - 1)
                    {
                        int bottomCellIndex = Convert.ToInt32((row + 1) * size.Width + column);
                        var edge1 = new Edge(Convert.ToInt32(row * size.Width + column), bottomCellIndex);
                        var edge2 = new Edge(bottomCellIndex, Convert.ToInt32(row * size.Width + column));
                        if (!Edges.Contains(edge1) && !Edges.Contains(edge2) && !cellEdges.Contains(edge2) && !rowEdges.Any(edges => edges.Contains(edge2)))
                        {
                            cellEdges.Add(edge1);
                        }
                    }


                    rowEdges.Add(cellEdges.ToArray());
                }

                PassableEdges.Add(rowEdges.SelectMany(edges => edges).ToList());
            }



            Start = 0; // Počáteční bod (můžete upravit dle potřeby)
            End = edgesToCheck.Count - 1; // Koncový bod (můžete upravit dle potřeby)
            MazeSolver ms = new MazeSolver(PassableEdges, Start, End, size);
            this.path = ms.FindPath();
            String strpath = "";
            if (path is not null)
            {
               
                for (int i = 0; i < path.Count; i++)
                {
                    strpath += " " + path[i];
                }
            }
            //Application.Current.MainPage.DisplayAlert("Upozornění", ms.VisitedCells.Count + "pocet", "OK");
            string uznevim = "";
            for (int i = 0; i < ms.VisitedCells.Count; i++)
            {
                uznevim += " " + ms.VisitedCells[i];
            }
            //Application.Current.MainPage.DisplayAlert("Upozornění", uznevim + "prostlo", "OK");
            //Application.Current.MainPage.DisplayAlert("Upozornění", strpath + "path", "OK");
            /*
            for (int i = 0; i < PassableEdges.Count; i++)
            {
                String str = "";
                for (int j = 0; j < PassableEdges[i].Count; j++)
                { 
                    str+=PassableEdges[i][j].Cell1+" "+ PassableEdges[i][j].Cell2 +"\n" ;    
                }
                Application.Current.MainPage.DisplayAlert("Upozornění", str+ "", "OK");
            }*/
           
        }

        public Maze(Size size, bool neco)
        {
            var edgesToCheck = Edge.GenerateCircularEdges(size);
            var sets = new DisjointSets(edgesToCheck.Count);
            var mazeEdges = new List<Edge>();

            var random = new Random();

            while (sets.Count > 1 && edgesToCheck.Count > 0)
            {
                // Pick a random edge
                var edgeIndex = random.Next(edgesToCheck.Count);
                var edge = edgesToCheck[edgeIndex];

                // Find the two sets separated by the edge
                var set1 = sets.Find(edge.Cell1);
                var set2 = sets.Find(edge.Cell2);

                if (set1 != set2)
                    sets.Union(set1, set2);
                else
                    mazeEdges.Add(edge);

                // Remove edge, so it won't be checked again
                edgesToCheck.RemoveAt(edgeIndex);
            }

            // Set maze properties
            Edges = edgesToCheck.Concat(mazeEdges).ToArray();
            Size = size;
        }
    }
}
