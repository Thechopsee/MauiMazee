using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiMaze.Drawables;
using MauiMaze.Engine;
using MauiMaze.Models.ClassicMaze;
using Microsoft.Maui;

namespace MazeUnitTests
{
    public class GameDriverTests
    {

        [Theory]
        [InlineData(20,20)]
        [InlineData(120,300)]
        [InlineData(70,50)]
        public void TestPlayerTake(float x,float y)
        {
            BaseMazeDrawable bd = new BaseMazeDrawable();
            Maze mz = new Maze(10,10);
            GraphicsView GV = new GraphicsView();
            GameDriver gm = new GameDriver(bd,mz);
            gm.setGraphicView(GV);
            bool moved=gm.movePlayerToPosition(x,y);
            Assert.False(moved);
        }
    }
}
