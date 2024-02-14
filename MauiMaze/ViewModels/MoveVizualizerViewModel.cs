using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiMaze.Drawables;
using MauiMaze.Engine;
using MauiMaze.Helpers;
using MauiMaze.Models;
using MauiMaze.Models.ClassicMaze;
using MauiMaze.Services;
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
        [ObservableProperty]
        public GraphicsView heatMapView;
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

        [ObservableProperty]
        public bool heatmapshow;
        [ObservableProperty]
        public bool vizualizershow;
        [ObservableProperty]
        public bool recordshow;

        [ObservableProperty]
        public bool vbs;
        [ObservableProperty]
        public bool hbs;
        [ObservableProperty]
        public bool gbs;

        public Maze maze { get; set; }
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
        [RelayCommand]
        public void switchView(string num)
        {
            int obj = Int32.Parse(num);
            switch (obj) {
                case 1:
                    Vizualizershow = true;
                    Heatmapshow = false;
                    Recordshow = false;
                    Vbs = false;
                    Hbs = true;
                    Gbs= true;
                    break;
                case 2:
                    Vizualizershow = false;
                    Heatmapshow = true;
                    Recordshow = false;
                    Vbs = true;
                    Hbs = false;
                    Gbs = true;
                    break;
                case 3:
                    Vizualizershow = false;
                    Heatmapshow = false;
                    Recordshow = true;
                    Vbs = true;
                    Hbs = true;
                    Gbs = false;
                    break;
            }
        }
        public void selectChanged(GameRecord gr)
        {
            if (Vizualizershow)
            {
                MazeDrawable md = (MazeDrawable)GraphicsView.Drawable;
                md.showAll = false;
                md.actualID = gr.grID;
                ShowAllEnabled = true;
                GraphicsView.Drawable = md;
                GraphicsView.Invalidate();
            }
            else if (Heatmapshow)
            {
                ActualGamerecord = gr;
                HeatmapDrawable md = (HeatmapDrawable)HeatMapView.Drawable;
                md.cellData=CountCellData(this.maze);
                HeatMapView.Drawable = md;
                HeatMapView.Invalidate();
            }
        }
        public MoveVizualizerViewModel(GraphicsView graphicsView,ListView listview,Maze maze,LoginCases lc, GraphicsView heatMapView)
        {
            Hbs = true;
            Gbs = true;
            Vizualizershow = true;
            cellEnabled = true;
            PositionEnabled = false;
            this.graphicsView = graphicsView;
            this.heatMapView= heatMapView;


            this.listview = listview;

            getRecordsAsync(maze,lc,heatMapView);



        }
        public CellData[] CountCellData(Maze maze) {

            this.maze = maze;
            List<CellData> cd=new List<CellData>();
            int maxcell = maze.Width * maze.Height;
            int maxtime = 0;
            for (int i = 0; i < maxcell; i++)
            {
                (int time,int hit)=filterDataForCells(i);
                if (time > maxtime)
                {
                    maxtime = time;
                }
                cd.Add(new CellData(i,Colors.Aqua,time,hit));
            }
            double sevfv = maxtime * 0.75;
            double fifty = maxtime * 0.50;
            double twfv= maxtime * 0.25;
            foreach (CellData c in cd)
            {
                if (c.time > sevfv)
                {
                    c.color = Colors.DarkRed;
                }
                else if (c.time > fifty)
                {
                    c.color = Colors.Red;
                }
                else if (c.time > twfv)
                {
                    c.color = Colors.Yellow;
                }
                else if (c.time > 0)
                {
                    c.color = Colors.Green;
                }
                else
                {
                    c.color = Colors.LightGreen;
                }
            }
            return cd.ToArray();
        }

        public (int,int) filterDataForCells(int cell)
        {
            int time = 0;
            int hit = 0;
            foreach (MoveRecord m in ActualGamerecord.moves)
            {
                if (m.cell == cell)
                {
                    time += m.deltaTinMilisec;
                    if (m.hitWall)
                    {
                        hit += 1;
                    }
                }
            }
            return (time,hit);
        }

        public async void getRecordsAsync(Maze maze,LoginCases lc, GraphicsView heatMapView)
        {
            if (lc == LoginCases.Offline)
            {
                gr = await RecordFetcher.loadRecordByMazeOffline(maze.MazeID).ConfigureAwait(true);
            }
            else
            {
                gr = await RecordFetcher.loadRecordsByMaze(maze.MazeID).ConfigureAwait(true);
            }
            
            MazeDrawable md = new MazeDrawable();

            for (int i = 0; i < gr.Count(); i++)
            {
                gr.ElementAt(i).color = ColorSchemeProvider.getColor(i);
                gr.ElementAt(i).grID = i;
            }
            if (gr.Count() > 0)
            {
                ActualGamerecord = gr.ElementAt(0);
                //md.preview = new Player(ActualGamerecord.moves[0].positionx, ActualGamerecord.moves[0].positiony, md.cellWidth, md.cellHeight);
                this.listview.ItemsSource = gr;
                md.gameRecords = gr;
            }
            md.maze = maze;
            GraphicsView.Drawable = md;
            CellData[] cd = CountCellData(maze);
            HeatMapView.Drawable = new HeatmapDrawable(maze, cd);
        }

        
    }
}
