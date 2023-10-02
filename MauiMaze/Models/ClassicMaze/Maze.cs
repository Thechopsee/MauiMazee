using MauiMaze.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMaze.Models.ClassicMaze
{
    public class Maze : GameMaze
    {
        public Edge[] Edges { get; set; }
        public List<List<Edge>> PassableEdges { get; set; }
        
        private void universalInit(Size size,List<Edge> edgesToCheck, DisjointSets sets)
        {
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
                
                edgesToCheck.RemoveAt(edgeIndex);
            }

            Edges = edgesToCheck.Concat(mazeEdges).ToArray();
            Size = size;
        }
        public Maze(Size size)
        {
            var edgesToCheck = Edge.Generate(size);
            var sets = new DisjointSets(Convert.ToInt32(size.Height * size.Width));
            universalInit(size, edgesToCheck, sets);
            // Set start and end points
            /*
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
            */
        }

        public Maze(Size size, bool neco)
        {
            var edgesToCheck = Edge.GenerateCircularEdges(size);
            var sets = new DisjointSets(edgesToCheck.Count);
            universalInit(size,edgesToCheck,sets);
        }
    }
}
