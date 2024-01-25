using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMaze.Models.RoundedMaze
{
    public class Cell
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public List<Link> Links { get; set; }
        public List<Link> Outward { get; set; }
        public Cell Cw { get; set; }
        public Cell Ccw { get; set; }
        public Cell Inward { get; set; }
        public float InnerCcwX { get; set; }
        public float InnerCcwY { get; set; }
        public float InnerCwX { get; set; }
        public float InnerCwY { get; set; }
        public float OuterCcwX { get; set; }
        public float OuterCcwY { get; set; }
        public float OuterCwX { get; set; }
        public float OuterCwY { get; set; }
        public float CenterX { get; set; }
        public float CenterY { get; set; }
    }

}
