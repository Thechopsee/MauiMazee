using MauiMaze.Models.ClassicMaze;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMaze.Models
{
    public class MazeDTO
    {
        public int size { get; set; }
        public int startCell { get; set; }
        public int endCell { get; set; }
        public Edge[] edges { get; set; }

    }
}
