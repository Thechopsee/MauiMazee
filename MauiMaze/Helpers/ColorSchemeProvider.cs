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
                            Colors.Black,
                            Colors.White,
                            Colors.Turquoise,
                            Colors.DarkTurquoise,
                            Colors.Wheat
            };
            if (i >= colorScheme.Length)
            {
                i = (i % colorScheme.Length);
            }
            return colorScheme[i];
        }
        public static Color getHeatmapColor(int value,int min,int max)
        {
            Color startColor = Color.FromRgb(105, 179, 76);   // Zelená barva pro minimální hodnotu
            Color middleColor1 = Color.FromRgb(250, 183, 51); // Žlutá barva
            Color middleColor2 = Color.FromRgb(255, 78, 17); // Oranžová barva
            Color endColor = Color.FromRgb(255, 13, 13);     // Červená barva pro maximální hodnotu

            double ratio = (double)(value - min) / (max - min);

            Color color;
            if (ratio == 0)
            {
                color = Colors.White;
            }
            else if (ratio < 0.25)
            {
                double subRatio = ratio / 0.25;
                int r = (int)((startColor.Red + subRatio * (middleColor1.Red - startColor.Red)) * 255);
                int g = (int)((startColor.Green + subRatio * (middleColor1.Green - startColor.Green)) * 255);
                int b = (int)((startColor.Blue + subRatio * (middleColor1.Blue - startColor.Blue))*255);
                color = Color.FromRgb(r, g, b);
            }
            else if (ratio < 0.5)
            {
                double subRatio = (ratio - 0.25) / 0.25;
                int r = (int)((middleColor1.Red + subRatio * (middleColor2.Red - middleColor1.Red)) * 255);
                int g = (int)((middleColor1.Green + subRatio * (middleColor2.Green - middleColor1.Green)) * 255);
                int b = (int)((middleColor1.Blue + subRatio * (middleColor2.Blue - middleColor1.Blue)) * 255);
                color = Color.FromRgb(r, g, b);
            }
            else if (ratio < 0.75)
            {
                double subRatio = (ratio - 0.5) / 0.25;
                int r = (int)((middleColor2.Red + subRatio * (endColor.Red - middleColor2.Red)) * 255);
                int g = (int)((middleColor2.Green + subRatio * (endColor.Green - middleColor2.Green)) * 255);
                int b = (int)((middleColor2.Blue + subRatio * (endColor.Blue - middleColor2.Blue))*255);
                color = Color.FromRgb(r, g, b);
            }
            else
            {
                double subRatio = (ratio - 0.75) / 0.25;
                int r = (int)((endColor.Red + subRatio * (1 - endColor.Red)) * 255);
                int g = (int)((endColor.Green + subRatio * (0 - endColor.Green)) * 255);
                int b = (int)((endColor.Blue + subRatio * (0 - endColor.Blue))*255);
                color = Color.FromRgb(r, g, b);
            }

            return color;
        }


    }
}
