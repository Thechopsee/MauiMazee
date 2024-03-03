using MauiMaze.Models.ClassicMaze;
using MauiMaze.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MauiMaze.Engine
{
    public class GameMaze :ICloneable
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int MazeID { get; set; }
        public End end { get; set; }
        public Start start { get; set; }
        public bool firstrun { get; set; }
        public Edge[] Edges { get; set; }
        public MazeType mazeType { get; set; }

        public object Clone()
        {
            string json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<GameMaze>(json);
        }
    }
}
