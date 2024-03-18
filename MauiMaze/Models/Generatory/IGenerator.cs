using MauiMaze.Models.ClassicMaze;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMaze.Models.Generatory
{
    public interface IGenerator
    {
        public List<Edge> GenerateMaze(int rows, int cols);
    }
}
