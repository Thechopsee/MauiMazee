using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiMaze.Engine;
using MauiMaze.Models.ClassicMaze;
using MauiMaze.Models.RoundedMaze;

namespace MazeUnitTests
{
    public class MazeTest
    {
        
        [Fact]
        public void canBeMazeSolvedSets()
        {
            Maze maze = new Maze(10, 10,MauiMaze.Helpers.GeneratorEnum.Sets);
            maze.start = new Start(-1, -1, 0);
            maze.end = new End(-1, -1, -1, -1, 84);
            Assert.True(SolveMaze(maze));   
        }
        [Fact]
        public void cantBeMazeSolvedSets()
        {
            Maze maze = new Maze(10, 10, MauiMaze.Helpers.GeneratorEnum.Sets);
            maze.start = new MauiMaze.Engine.Start(-1, -1, 0);
            maze.end = new MauiMaze.Engine.End(-1, -1, -1, -1, 100);
            Assert.False(SolveMaze(maze));
        }
        [Fact]
        public void canBeMazeSolvedHNK()
        {
            Maze maze = new Maze(10, 10, MauiMaze.Helpers.GeneratorEnum.HuntNKill);
            maze.start = new MauiMaze.Engine.Start(-1, -1, 0);
            maze.end = new MauiMaze.Engine.End(-1, -1, -1, -1, 84);
            Assert.True(SolveMaze(maze));
        }
        [Fact]
        public void cantBeMazeSolvedHNK()
        {
            Maze maze = new Maze(10, 10, MauiMaze.Helpers.GeneratorEnum.HuntNKill);
            maze.start = new MauiMaze.Engine.Start(-1, -1, 0);
            maze.end = new MauiMaze.Engine.End(-1, -1, -1, -1, 100);
            Assert.False(SolveMaze(maze));
        }
        [Fact]
        public void canBeMazeSolvedSets_R()
        {
            GameMaze maze = new RoundedMaze(new Size(10,10), MauiMaze.Helpers.GeneratorEnum.Sets);
            maze.start = new MauiMaze.Engine.Start(-1, -1, 0);
            maze.end = new MauiMaze.Engine.End(-1, -1, -1, -1, 84);
            Assert.True(SolveMaze(maze));
        }
        [Fact]
        public void cantBeMazeSolvedSets_R()
        {
            GameMaze maze = new RoundedMaze(new Size(10, 10), MauiMaze.Helpers.GeneratorEnum.Sets);
            maze.start = new MauiMaze.Engine.Start(-1, -1, 0);
            maze.end = new MauiMaze.Engine.End(-1, -1, -1, -1, 100);
            Assert.False(SolveMaze(maze));
        }
        [Fact]
        public void canBeMazeSolvedHNK_R()
        {
            GameMaze maze = new RoundedMaze(new Size(10, 10), MauiMaze.Helpers.GeneratorEnum.HuntNKill);
            maze.start = new MauiMaze.Engine.Start(-1, -1, 0);
            maze.end = new MauiMaze.Engine.End(-1, -1, -1, -1, 84);
            Assert.True(SolveMaze(maze));
        }
        [Fact]
        public void cantBeMazeSolvedHNK_R()
        {
            GameMaze maze = new RoundedMaze(new Size(10, 10), MauiMaze.Helpers.GeneratorEnum.HuntNKill);
            maze.start = new MauiMaze.Engine.Start(-1, -1, 0);
            maze.end = new MauiMaze.Engine.End(-1, -1, -1, -1, 100);
            Assert.False(SolveMaze(maze));
        }

        public bool SolveMaze(GameMaze maze)
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
                    return true; 
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

            return false; 
        }

        private List<int> GetAdjacentCells(int cellIndex,GameMaze maze)
        {
            var adjacentCells = new List<int>();

            if (cellIndex - maze.Width >= 0)
            {
                adjacentCells.Add(cellIndex - maze.Width);
            }

            if (cellIndex + maze.Width < maze.Width * maze.Height)
            {
                adjacentCells.Add(cellIndex + maze.Width);
            }

            if (cellIndex % maze.Width != 0)
            {
                adjacentCells.Add(cellIndex - 1);
            }

            if ((cellIndex + 1) % maze.Width != 0)
            {
                adjacentCells.Add(cellIndex + 1);
            }

            return adjacentCells;
        }
    }

}
