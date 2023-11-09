using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMaze.Exceptions
{
    public class MazeNotLoadedExpectation : Exception
    {
        public MazeNotLoadedExpectation(string message) : base(message)
        {
        }
    }
}
