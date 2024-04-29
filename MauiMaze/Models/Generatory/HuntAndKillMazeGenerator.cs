using MauiMaze.Models.ClassicMaze;
using System;
using System.Collections.Generic;

namespace MauiMaze.Models.Generatory
{
    public class HuntAndKillMazeGenerator :IGenerator
    {
        public List<Edge> GenerateMaze(int rows, int cols)
        {
            var edges = new List<Edge>();
            var random = new Random();

            var cell = 0;
            for (var row = 0; row < rows; row++)
            {
                for (var column = 0; column < cols; column++)
                {
                    if (column != rows - 1)
                    {
                        edges.Add(new Edge(cell, cell + 1));
                    }

                    if (row != cols - 1)
                    {
                        edges.Add(new Edge(cell, cell + Convert.ToInt32(cols)));
                    }
                    cell++;
                }
            }
            var visited = new bool[rows, cols];
            int currentRow = random.Next(rows);
            int currentCol = random.Next(cols);

            while (true)
            {
                visited[currentRow, currentCol] = true;

                var unvisitedNeighbors = GetUnvisitedNeighbors(currentRow, currentCol, visited, rows, cols);

                if (unvisitedNeighbors.Count > 0)
                {
                    var randomNeighbor = unvisitedNeighbors[random.Next(unvisitedNeighbors.Count)];
                    int neighborRow = randomNeighbor.Item1;
                    int neighborCol = randomNeighbor.Item2;

                    int currentCell = currentRow * cols + currentCol;
                    int neighborCell = neighborRow * cols + neighborCol;
                    edges.RemoveAll(edge => edge.Cell1 == currentCell && edge.Cell2 == neighborCell || edge.Cell1 == neighborCell && edge.Cell2 == currentCell);

                    currentRow = neighborRow;
                    currentCol = neighborCol;
                }
                else
                {
                    bool found = false;
                    for (int r = 0; r < rows && !found; r++)
                    {
                        for (int c = 0; c < cols && !found; c++)
                        {
                            (bool has, int frow, int fcol) = HasVisitedNeighbor(r, c, visited, rows, cols);
                            if (!visited[r, c] && has)
                            {
                                int currentCell = frow * cols + fcol;
                                int newCell = r * cols + c;
                                edges.RemoveAll(edge => edge.Cell1 == currentCell && edge.Cell2 == newCell || edge.Cell1 == newCell && edge.Cell2 == currentCell);

                                visited[r, c] = true;

                                currentRow = r;
                                currentCol = c;
                                found = true;
                            }
                        }
                    }
                    if (!found)
                        break;
                }
            }
            return edges;
        }

        private static List<Tuple<int, int>> GetUnvisitedNeighbors(int row, int col, bool[,] visited, int rows, int cols)
        {
            var unvisitedNeighbors = new List<Tuple<int, int>>();

            if (row > 0 && !visited[row - 1, col])
                unvisitedNeighbors.Add(new Tuple<int, int>(row - 1, col));

            if (row < rows - 1 && !visited[row + 1, col])
                unvisitedNeighbors.Add(new Tuple<int, int>(row + 1, col));

            if (col > 0 && !visited[row, col - 1])
                unvisitedNeighbors.Add(new Tuple<int, int>(row, col - 1));

            if (col < cols - 1 && !visited[row, col + 1])
                unvisitedNeighbors.Add(new Tuple<int, int>(row, col + 1));

            return unvisitedNeighbors;
        }

        private static (bool, int, int) HasVisitedNeighbor(int row, int col, bool[,] visited, int rows, int cols)
        {
            if (row > 0 && visited[row - 1, col])
                return (true, row - 1, col);

            if (row < rows - 1 && visited[row + 1, col])
                return (true, row + 1, col);

            if (col > 0 && visited[row, col - 1])
                return (true, row, col - 1);

            if (col < cols - 1 && visited[row, col + 1])
                return (true, row, col + 1);

            return (false, -1, -1);
        }
    }
}
