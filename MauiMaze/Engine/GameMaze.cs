using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMaze.Engine
{
    public class GameMaze
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int MazeID { get; set; }
        public End end { get; set; }
        public Start start { get; set; }
        public bool firstrun { get; set; }
    }
}
