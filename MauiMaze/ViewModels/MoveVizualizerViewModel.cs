using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiMaze.Drawables;
using MauiMaze.Engine;
using MauiMaze.Helpers;
using MauiMaze.Models;
using MauiMaze.Models.ClassicMaze;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMaze.ViewModels
{
    public partial class MoveVizualizerViewModel : ObservableObject
    {
        [ObservableProperty]
        public GraphicsView graphicsView;
        IEnumerable<GameRecord> gr;
        [ObservableProperty]
        public GameRecord actualGamerecord;
        private ListView listview;
        [ObservableProperty]
        public int movePercentage;
        [ObservableProperty]
        public bool cellEnabled;
        [ObservableProperty]
        public bool positionEnabled;
        [ObservableProperty]
        public bool showAllEnabled;
        [RelayCommand]
        public void valueChanged()
        {
            int mvp=(ActualGamerecord.moves.Count() / 100) * MovePercentage;
            MazeDrawable md = (MazeDrawable)GraphicsView.Drawable;
            md.preview.positionX = ActualGamerecord.moves[mvp].positionx;
            md.preview.positionY = ActualGamerecord.moves[mvp].positiony;
            md.preview.playerSizeX = md.cellWidth;
            md.preview.playerSizeY = md.cellHeight;
            GraphicsView.Invalidate();
        }
        [RelayCommand]
        public void switchToCell()
        {
            CellEnabled = false;
            PositionEnabled = true;
            MazeDrawable md = (MazeDrawable)GraphicsView.Drawable;
            md.showCell = true;
            GraphicsView.Drawable = md;
            GraphicsView.Invalidate();
        }
        [RelayCommand]
        public void switchToPosition()
        {
            PositionEnabled = false;
            CellEnabled = true;
            MazeDrawable md = (MazeDrawable)GraphicsView.Drawable;
            md.showCell = false;
            GraphicsView.Drawable = md;
            GraphicsView.Invalidate();
        }
        [RelayCommand]
        public void switchToAll()
        {
            ShowAllEnabled = false;
            MazeDrawable md = (MazeDrawable)GraphicsView.Drawable;
            md.showAll = true;
            GraphicsView.Drawable = md;
            GraphicsView.Invalidate();
        }
        public void selectChanged(GameRecord gr)
        {
            MazeDrawable md = (MazeDrawable)GraphicsView.Drawable;
            md.showAll = false;
            md.actualID = gr.grID;
            ShowAllEnabled = true;
            GraphicsView.Drawable = md;
            GraphicsView.Invalidate();
        }
        public MoveVizualizerViewModel(GraphicsView graphicsView,ListView listview,Maze maze)
        {
            cellEnabled = true;
            PositionEnabled = false;
            this.graphicsView = graphicsView;
            MazeDrawable md=new MazeDrawable();
            this.listview = listview;
            
            gr= RecordRepository.GetInstance().getRecordsByMazeId(maze.MazeID);
            for(int i=0;i< gr.Count();i++)
            {
                gr.ElementAt(i).color = ColorSchemeProvider.getColor(i); 
                gr.ElementAt(i).grID = i;
            }
            actualGamerecord = gr.ElementAt(0);
            md.preview = new Player(actualGamerecord.moves[0].positionx, actualGamerecord.moves[0].positiony,md.cellWidth,md.cellHeight);
            this.listview.ItemsSource=gr;
            md.gameRecords = gr;
            md.maze = maze;

            this.graphicsView.Drawable = md;

        }
        
    }
}
