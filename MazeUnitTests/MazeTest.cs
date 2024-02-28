using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiMaze.Models.ClassicMaze;

namespace MazeUnitTests
{
    public class MazeTest
    {
        
        [Fact]
        public void canBeMazeSolved()
        {
            Maze maze = new Maze(10, 10);
            maze.start = new MauiMaze.Engine.Start(-1, -1, 0);
            maze.end = new MauiMaze.Engine.End(-1, -1, -1, -1, 84);
            Assert.True(SolveMaze(maze));   
        }
        [Fact]
        public void cantBeMazeSolved()
        {
            Maze maze = new Maze(10, 10);
            maze.start = new MauiMaze.Engine.Start(-1, -1, 0);
            maze.end = new MauiMaze.Engine.End(-1, -1, -1, -1, 100);
            Assert.False(SolveMaze(maze));
        }
        public bool SolveMaze(Maze maze)
        {
            var visited = new HashSet<int>();
            var queue = new Queue<int>();

            queue.Enqueue(maze.start.cell);

            while (queue.Any())
            {
                var currentCell = queue.Dequeue();
                visited.Add(currentCell);

                if (currentCell == maze.end.cell)
                {
                    return true; // Path found
                }

                var adjacentCells = GetAdjacentCells(currentCell,maze);

                foreach (var nextCell in adjacentCells)
                {
                    if (!visited.Contains(nextCell))
                    {
                        queue.Enqueue(nextCell);
                    }
                }
            }

            return false; // No path found
        }

        private List<int> GetAdjacentCells(int cellIndex,Maze maze)
        {
            var adjacentCells = new List<int>();

            // Check north
            if (cellIndex - maze.Width >= 0)
            {
                adjacentCells.Add(cellIndex - maze.Width);
            }

            // Check south
            if (cellIndex + maze.Width < maze.Width * maze.Height)
            {
                adjacentCells.Add(cellIndex + maze.Width);
            }

            // Check west
            if (cellIndex % maze.Width != 0)
            {
                adjacentCells.Add(cellIndex - 1);
            }

            // Check east
            if ((cellIndex + 1) % maze.Width != 0)
            {
                adjacentCells.Add(cellIndex + 1);
            }

            return adjacentCells;
        }
    }

}
