using MauiMaze.Drawables;
using MauiMaze.Helpers;
using MauiMaze.Models;
using MauiMaze.Models.ClassicMaze;
using MauiMaze.Models.Generatory;
using MauiMaze.ViewModels;
using MazeUnitTests.Mock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeUnitTests.UITests
{
    public class VizualizationTests
    {

        [Fact]
        public void SwitchToPositionUpdatesPropertiesAndDrawable()
        {
            var viewModel = new MoveVizualizerViewModel();
            viewModel.GraphicsView = new GraphicsView();
            viewModel.GraphicsView.Drawable = new MoveVizualizerDrawable();
            viewModel.PositionEnabled = false;

            viewModel.switchToPosition();

            Assert.False(viewModel.PositionEnabled);
            Assert.True(viewModel.CellEnabled);
            var drawable = viewModel.GraphicsView.Drawable as MoveVizualizerDrawable;
            Assert.NotNull(drawable);
            Assert.False(drawable.showCell);
        }
        

        [Fact]
        public async void SwitchToTimeUpdatesPropertiesAndDrawable()
        {
            var viewModel = new MoveVizualizerViewModel();
            viewModel.HeatMapView = new GraphicsView();
            var maze = new Maze(10,10,GeneratorEnum.Sets);
            var cellData = new CellData[10];
            viewModel.maze = maze;
            viewModel.HeatMapView.Drawable = new HeatmapDrawable(maze ,cellData);
            viewModel.TimeEnabled = false;
      
            MauiMaze.Engine.GameRecord gr = await GameRecordMocker.getMock(new Size(500, 500), 500 / 10, 100);
            viewModel.ActualGamerecord = gr;
           

            viewModel.switchToTime();

            Assert.True(viewModel.HitsEnabled);
            Assert.False(viewModel.TimeEnabled);
            Assert.True(viewModel.AllHeatEnabled);
            var drawable = viewModel.HeatMapView.Drawable as HeatmapDrawable;
            Assert.NotNull(drawable);
            Assert.True(drawable.time);
            Assert.False(drawable.hits);
        }

        [Fact]
        public async void SwitchToHitsUpdatesPropertiesAndDrawable()
        {

            var viewModel = new MoveVizualizerViewModel();
            viewModel.HeatMapView = new GraphicsView();
            var maze = new Maze(10, 10, GeneratorEnum.Sets);

            viewModel.HitsEnabled = false;
            var cellData = new CellData[10];

            viewModel.maze = maze;
            MauiMaze.Engine.GameRecord gr = await GameRecordMocker.getMock(new Size(500, 500), 500 / 10, 100);
            viewModel.ActualGamerecord = gr;
            viewModel.HeatMapView.Drawable = new HeatmapDrawable(maze, cellData);
            viewModel.switchToHits();

            Assert.True(viewModel.TimeEnabled);
            Assert.False(viewModel.HitsEnabled);
            Assert.True(viewModel.AllHeatEnabled);
            var drawable = viewModel.HeatMapView.Drawable as HeatmapDrawable;
            Assert.NotNull(drawable);
            Assert.False(drawable.time);
            Assert.True(drawable.hits);
        }

        [Fact]
        public async void SwitchToAllHeatUpdatesPropertiesAndDrawable()
        {
            var viewModel = new MoveVizualizerViewModel();
            viewModel.HeatMapView = new GraphicsView();
            var maze = new Maze(10, 10, GeneratorEnum.Sets);
            viewModel.maze = maze;
            MauiMaze.Engine.GameRecord gr = await GameRecordMocker.getMock(new Size(500, 500), 500 / 10, 100);
            viewModel.ActualGamerecord = gr;
            var cellData = new CellData[10];
            viewModel.HeatMapView.Drawable = new HeatmapDrawable(maze, cellData);
            viewModel.AllHeatEnabled = false;
            viewModel.switchToAllHeat();

            Assert.True(viewModel.HitsEnabled);
            Assert.True(viewModel.TimeEnabled);
            Assert.False(viewModel.AllHeatEnabled);
            var drawable = viewModel.HeatMapView.Drawable as HeatmapDrawable;
            Assert.NotNull(drawable);
            Assert.True(drawable.time);
            Assert.True(drawable.hits);

        }


    }
}
