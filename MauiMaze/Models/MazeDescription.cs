using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMaze.Models
{
    public class MazeDescription
    {
        public int ID { get; set; }
        public MazeType mazeType { get; set; }
        public DateTime creationDate { get; set; }

        public string description { get; set; }

    }
}
