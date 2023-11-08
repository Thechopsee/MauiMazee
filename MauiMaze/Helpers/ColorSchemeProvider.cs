using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMaze.Helpers
{
    public class ColorSchemeProvider
    {
        public static Color getColor(int i)
        {
            Color[] colorScheme = new Color[]
            {
                            Colors.Red,
                            Colors.Blue,
                            Colors.Green,
                            Colors.Yellow,
                            Colors.Orange,
                            Colors.Purple,
                            Colors.Cyan,
                            Colors.Magenta,
                            Colors.Gray,
                            Colors.Black
            };
            return colorScheme[i];
        }
    }
}
