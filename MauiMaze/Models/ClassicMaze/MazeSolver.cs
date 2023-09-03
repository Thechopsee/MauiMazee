namespace MauiMaze.Models.ClassicMaze
{
    public class MazeSolver
    {
        private readonly List<List<Edge>> PassableEdges;
        private readonly int Start;
        private readonly int End;
        public readonly List<int> VisitedCells;
        private readonly Size Size;

        public MazeSolver(List<List<Edge>> passableEdges, int start, int end, Size size)
        {
            PassableEdges = passableEdges;
            Start = start;
            End = end;
            VisitedCells = new List<int>();
            Size = size;
        }

        public List<int> FindPath()
        {
            return FindPath(0, 0, new List<int>());
        }

        private List<int> FindPath(int row, int column, List<int> path)
        {
            int currentCell = Convert.ToInt32(row * Size.Width + column);

            // Add current cell to the path
            path.Add(currentCell);

            // Mark current cell as visited
            VisitedCells.Add(currentCell);

            // Check if current cell is the end cell
            if (currentCell == End)
            {
                return path;
            }

            // Find passable neighbors of the current cell
            var neighbors = GetPassableNeighbors(row, column);

            // Recursively explore the neighboring cells
            foreach (var neighbor in neighbors)
            {
                int neighborRow = Convert.ToInt32(neighbor / Convert.ToInt32(Size.Width));
                int neighborColumn = Convert.ToInt32(neighbor % Convert.ToInt32(Size.Height));

                if (!VisitedCells.Contains(neighbor))
                {
                    var newPath = FindPath(neighborRow, neighborColumn, path.ToList());
                    if (newPath != null)
                    {
                        return newPath;
                    }
                }
            }

            return null; // Path not found
        }

        private List<int> GetPassableNeighbors(int row, int column)
        {
            int currentCellIndex = Convert.ToInt32(row * Size.Width + column);
            if (row == Size.Height)
            {
                return new List<int>();
            }
            var currentCellEdges = PassableEdges[row]; // Kolekce hran pro aktuální buňku
            var neighbors = new List<int>();
            /*
            String strpath = "";
            for (int i = 0; i < currentCellEdges.Count; i++)
            {
                strpath += " " + currentCellEdges[i].Cell1+"->"+ currentCellEdges[i].Cell2;
            }

            Application.Current.MainPage.DisplayAlert("Upozornění", "curentcells\n" +strpath + "", "OK"); 
            */
            // Top neighbor
            if (row > 0 && currentCellEdges.Any())
            {
                int topCellIndex = Convert.ToInt32((row - 1) * Size.Width + column);
                if (currentCellEdges.Any(edge => edge.Cell1 == currentCellIndex && edge.Cell2 == topCellIndex))
                {
                    neighbors.Add(topCellIndex);
                }
            }

            // Bottom neighbor
            if (row < Size.Height - 1 && currentCellEdges.Any())
            {
                int bottomCellIndex = Convert.ToInt32((row + 1) * Size.Width + column);
                if (currentCellEdges.Any(edge => edge.Cell1 == currentCellIndex && edge.Cell2 == bottomCellIndex))
                {
                    neighbors.Add(bottomCellIndex);
                }
            }

            // Left neighbor
            if (column > 0 && currentCellEdges.Any())
            {
                int leftCellIndex = Convert.ToInt32(row * Size.Width + (column - 1));
                if (currentCellEdges.Any(edge => edge.Cell1 == currentCellIndex && edge.Cell2 == leftCellIndex))
                {
                    neighbors.Add(leftCellIndex);
                }
            }

            // Right neighbor
            if (column < Size.Width - 1 && currentCellEdges.Any())
            {
                int rightCellIndex = Convert.ToInt32(row * Size.Width + (column + 1));
                if (currentCellEdges.Any(edge => edge.Cell1 == currentCellIndex && edge.Cell2 == rightCellIndex))
                {
                    neighbors.Add(rightCellIndex);
                }
            }
            return neighbors;
        }


    }
}