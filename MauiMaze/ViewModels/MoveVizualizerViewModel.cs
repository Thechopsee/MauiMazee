﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiMaze.Drawables;
using MauiMaze.Engine;
using MauiMaze.Helpers;
using MauiMaze.Models;
using MauiMaze.Models.ClassicMaze;
using MauiMaze.Services;
using Newtonsoft.Json.Linq;
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
        public bool cellEnabled;
        [ObservableProperty]
        public bool positionEnabled;
        [ObservableProperty]
        public bool showAllEnabled;
        [ObservableProperty]
        public bool timeEnabled;
        [ObservableProperty]
        public bool hitsEnabled;
        [ObservableProperty]
        public bool allHeatEnabled;
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
        private LoginCases lc { get; set; }
        [RelayCommand]
        public void switchToCell()
        {
            CellEnabled = false;
            PositionEnabled = true;
            MoveVizualizerDrawable md = (MoveVizualizerDrawable)GraphicsView.Drawable;
            md.showCell = true;
            GraphicsView.Drawable = md;
            GraphicsView.Invalidate();
        }
        [RelayCommand]
        public void switchToPosition()
        {
            PositionEnabled = false;
            CellEnabled = true;
            MoveVizualizerDrawable md = (MoveVizualizerDrawable)GraphicsView.Drawable;
            md.showCell = false;
            GraphicsView.Drawable = md;
            GraphicsView.Invalidate();
        }
        [RelayCommand]
        public async Task play()
        {
            MoveVizualizerDrawable md = (MoveVizualizerDrawable)GraphicsView.Drawable;
            Player player = new Player(0, 0, 0, 0);
            md.mazeDrawable.reinitPlayer(player);
            md.mazeDrawable.player = player;
            player.dummy = true;
            GraphicsView.Drawable = md;
            GraphicsView.Invalidate();
            GraphicsView.Invalidate();
            GraphicsView.Invalidate();
            await Task.Run(async () =>
            {
                for (int i = 0; i < ActualGamerecord.moves.Count; i++)
                {
                    await Task.Delay(ActualGamerecord.moves[i].deltaTinMilisec);
                    (int newX,int newY)=MoveVizualizerDrawable.GetPlayerPositionInMazeSize(ActualGamerecord.moves[i].percentagex, ActualGamerecord.moves[i].percentagey, (int)md.mazeDrawable.mazeWidth,(int) md.mazeDrawable.mazeHeight);
                    md.mazeDrawable.player.positionX = (int)(newX-(md.mazeDrawable.cellWidth/2));
                    md.mazeDrawable.player.positionY = (int)(newY - (md.mazeDrawable.cellHeight/2));
                    GraphicsView.Invalidate();
                }
            });

        }
        [RelayCommand]
        public void switchToTime()
        {
            TimeEnabled = false;
            HitsEnabled= true;
            AllHeatEnabled = true;
            HeatmapDrawable md = (HeatmapDrawable)HeatMapView.Drawable;
            md.cellData = CountCellData(this.maze, ActualGamerecord);
            md.time = true;
            md.hits = false;
            HeatMapView.Drawable = md;
            HeatMapView.Invalidate();
        }
        [RelayCommand]
        public void switchToHits()
        {
            HitsEnabled= false;
            TimeEnabled = true;
            AllHeatEnabled = true;
            HeatmapDrawable md = (HeatmapDrawable)HeatMapView.Drawable;
            md.cellData = CountCellData(this.maze, ActualGamerecord);
            md.time = false;
            md.hits = true;
            HeatMapView.Drawable = md;
            HeatMapView.Invalidate();
        }
        [RelayCommand]
        public void switchToAllHeat()
        {
            HitsEnabled = true;
            TimeEnabled = true;
            AllHeatEnabled = false;
            HeatmapDrawable md = (HeatmapDrawable)HeatMapView.Drawable;
            md.cellData = CountCellData(this.maze, ActualGamerecord);
            md.time = true;
            md.hits = true;
            HeatMapView.Drawable = md;
            HeatMapView.Invalidate();
        }

        [RelayCommand]
        public void switchToAll()
        {
            if (Vizualizershow)
            {
                ShowAllEnabled = false;
                MoveVizualizerDrawable md = (MoveVizualizerDrawable)GraphicsView.Drawable;
                md.showAll = true;
                GraphicsView.Drawable = md;
                GraphicsView.Invalidate();
            }
            else if (Heatmapshow)
            {
                ShowAllEnabled = false;
                List<CellData> cdl = new();
                for (int i = 0; i < maze.Width * maze.Height; i++)
                {
                    cdl.Add(new CellData(i, Colors.Green, 0, 0));
                }
                foreach (GameRecord record in gr)
                {
                    CellData[] cd = CountCellData(maze, record);
                    for (int i = 0; i < cd.Length; i++)
                    {
                        cdl[i].time += cd[i].time;
                        cdl[i].hit += cd[i].hit;
                    }
                }
                int maxtime = 0;
                int maxhits = 0;
                foreach (CellData data in cdl)
                {
                    data.time = data.time / gr.Count();
                    data.hit = data.hit / gr.Count();
                    if (data.time > maxtime)
                    {
                        maxtime = data.time;
                    }
                    if (data.hit > maxhits)
                    { 
                        maxhits= data.hit;
                    }
                }
                foreach (CellData c in cdl)
                {
                    if (!HitsEnabled)
                    {
                        if (maxhits == 0)
                        {
                            c.color = Colors.White;
                        }
                        else
                        {
                            c.color = ColorSchemeProvider.getHeatmapColor(c.hit, 0, maxhits);
                        }
                    }
                    else
                    {
                        c.color = ColorSchemeProvider.getHeatmapColor(c.time, 0, maxtime);
                    }
                }
                HeatMapView.Drawable = new HeatmapDrawable(maze, cdl.ToArray());
            }
            else if (Recordshow)
            {
                ShowAllEnabled=false;
                int avarageHit = 0;
                int avaragetime = 0;
                foreach (GameRecord record in gr)
                {
                    avarageHit += record.hitWallsCount;
                    avaragetime += record.timeInMilliSeconds;
                    
                }
                avarageHit = avarageHit / gr.Count();
                avaragetime = avaragetime / gr.Count();
                GameRecord temp = new GameRecord(-1, -1);
                temp.timeInMilliSeconds = avaragetime;
                temp.hitWallsCount = avarageHit;
                ActualGamerecord = temp;
            }
        }
        [RelayCommand]
        public void switchView(string num)
        {
            int obj = Int32.Parse(num);
            if (lc == LoginCases.Offline)
            {
                return;
            }
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
            ActualGamerecord = gr;
            ShowAllEnabled = true;
            if (Vizualizershow)
            {
                MoveVizualizerDrawable md = (MoveVizualizerDrawable)GraphicsView.Drawable;
                md.showAll = false;
                md.actualID = gr.grID;
                GraphicsView.Drawable = md;
                GraphicsView.Invalidate();
            }
            else if (Heatmapshow)
            {
                HeatmapDrawable md = (HeatmapDrawable)HeatMapView.Drawable;
                md.cellData=CountCellData(this.maze, ActualGamerecord);
                HeatMapView.Drawable = md;
                HeatMapView.Invalidate();
            }
        }
        public MoveVizualizerViewModel(GraphicsView graphicsView, ListView listview, Maze maze, LoginCases lc, GraphicsView heatMapView)
        {
            Hbs = true;
            Gbs = true;
            Vizualizershow = true;
            cellEnabled = true;
            PositionEnabled = false;
            AllHeatEnabled = false;
            TimeEnabled = true;
            HitsEnabled = true;
            this.graphicsView = graphicsView;
            this.heatMapView = heatMapView;
            this.listview = listview;
            this.lc = lc;
            if (lc == LoginCases.Offline)
            {
                Hbs = false;
                Gbs = true;
                Vbs = false;
                Vizualizershow = false;
                Recordshow = true;

            }
            getRecordsAsync(maze, lc, heatMapView);

        }
        public MoveVizualizerViewModel()
        {
            if (Application.Current is not null)
            {
                throw new Exception("only for test");
            }
        }
        public CellData[] CountCellData(Maze maze,GameRecord record) {

            this.maze = maze;
            List<CellData> cd=new List<CellData>();
            int maxcell = maze.Width * maze.Height;
            int maxtime = 0;
            int maxhits = 0;
            if (record is null)
            {
                return new List<CellData>().ToArray();
            }
            for (int i = 0; i < maxcell; i++)
            {
                (int time,int hit)=filterDataForCells(i, record);
                if (time > maxtime)
                {
                    maxtime = time;
                }
                if (hit > maxhits)
                { 
                    maxhits = hit; 
                }
                
                cd.Add(new CellData(i,Colors.Aqua,time,hit));
            }
            foreach (CellData c in cd)
            {
                if (!HitsEnabled)
                {
                    if (maxhits == 0)
                    {
                        c.color = Colors.White;
                    }
                    else
                    {
                        c.color = ColorSchemeProvider.getHeatmapColor(c.hit, 0, maxhits);
                    }
                }
                else
                {
                    c.color = ColorSchemeProvider.getHeatmapColor(c.time, 0, maxtime);
                }
            }
            return cd.ToArray();
        }

        public (int,int) filterDataForCells(int cell,GameRecord record)
        {
            int time = 0;
            int hit = 0;
            foreach (MoveRecord m in record.moves)
            {
                if (m.cell == cell)
                {
                    time += m.deltaTinMilisec;
                    if (m.hitWall == 1)
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
            MoveVizualizerDrawable md = new MoveVizualizerDrawable();

            for (int i = 0; i < gr.Count(); i++)
            {
                gr.ElementAt(i).color = ColorSchemeProvider.getColor(i);
                gr.ElementAt(i).grID = i;
            }
            if (gr.Count() > 0)
            {
                ActualGamerecord = gr.ElementAt(0);
                this.listview.ItemsSource = gr;
                md.gameRecords = gr.ToList();
            }
            md.sendMaze(maze);
            GraphicsView.Drawable = md;
            CellData[] cd = CountCellData(maze, ActualGamerecord);
            HeatMapView.Drawable = new HeatmapDrawable(maze, cd);
        }
    }
}
