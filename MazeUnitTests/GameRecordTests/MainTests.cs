using MauiMaze.Engine;
using MauiMaze.Helpers;
using MauiMaze.Models.ClassicMaze;
using MauiMaze.ViewModels;
using MazeUnitTests.Mock;
using RichardSzalay.MockHttp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MazeUnitTests.GameRecordTests
{
    public class gamerecordTests
    {
        
        [Fact]
        public async Task GameRecordtestobuild()
        {
            MauiMaze.Engine.GameRecord gr =await GameRecordMocker.getMock(new Size(500,500),500/10,1000);
            Assert.NotNull(gr);
            Assert.NotEmpty(gr.moves);
            Assert.Equal(1000,gr.moves.Count);
            Assert.NotEmpty(gr.cellPath);
            Assert.False(gr.stopwatch.IsRunning);
            Assert.NotEqual(0, gr.hitWallsCount);
            Assert.NotEqual(0, gr.timeInMilliSeconds);
        }
        [Fact]
        public async Task optimalizationTest()
        {
            MoveVizualizerViewModel mvvm = new MoveVizualizerViewModel();
            Assert.NotNull(mvvm);
            GameRecord gr = new GameRecord(-1, -1);
            for (int i = 0; i < 5; i++)
            {
                gr.addMoveRecord(new MoveRecord(0, 0.1, 0.1, 1, 1, 1));
            }
            gr.addMoveRecord(new MoveRecord(0, 0.5, 0.5, 0, 10, 1));
            Maze maze = new Maze(10, 10);
            CellData[] cd = mvvm.CountCellData(maze, gr);
            Assert.NotNull(cd);
            Assert.Equal(11, cd[1].time);
            Assert.Equal(1, cd[1].hit);
        }

        [Fact]
        public async Task heatmapCountingTest()
        {
            MoveVizualizerViewModel mvvm = new MoveVizualizerViewModel();
            Assert.NotNull(mvvm);
            GameRecord gr = new GameRecord(-1, -1);
            for (int i = 0; i < 5; i++)
            {
                gr.addMoveRecord(new MoveRecord(0, 0.1+((float)i/100), 0.1+((float)i / 100), 1, 2, 1));
            }
            Maze maze = new Maze(10,10);
            CellData[] cd=mvvm.CountCellData(maze,gr);
            Assert.NotNull(cd);
            Assert.Equal(10, cd[1].time);
            Assert.Equal(5, cd[1].hit);
        }
    }
}
