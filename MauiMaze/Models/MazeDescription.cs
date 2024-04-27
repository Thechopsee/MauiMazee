using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiMaze.Engine;
using MauiMaze.Models.ClassicMaze;
using MauiMaze.Models.RoundedMaze;
using MauiMaze.Models.DTOs;
using MauiMaze.Services;
using MauiMaze.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMaze.Models
{
    public partial class MazeDescription : ObservableObject
    {
        public int ID { get; set; }
        public MazeType mazeType { get; set; }
        public DateTime creationDate { get; set; }
        public LoginCases whereIsMazeSaved { get; set; }

        [ObservableProperty]
        bool offline;
        public string description { get; set; }

        private bool local = false;

        public MazeDescription() { }
        public MazeDescription(int id,MazeType mt,DateTime cD,LoginCases lc) {
            this.ID = id;
            this.mazeType = mt;
            this.creationDate = cD;
            this.local = true;
            this.description = "" + ID + mt;
            this.whereIsMazeSaved = lc;
            
            if (UserDataProvider.GetInstance().getUserID() != -1)
            {
                Offline = true;
            }
            if (mazeType == MazeType.Rounded)
            {
                offline = false;
            }
        }

        [RelayCommand]
        public async Task GoToMoves()
        {
            GameMaze mz;
            if (local && ID >= 0)
            {
                 mz= await MazeFetcher.getMazeLocalbyID(ID);
            }
            else
            {
                 mz = await MazeFetcher.getMaze(ID).ConfigureAwait(true);
            }
            mz.MazeID = ID;
            Maze maz = new Maze(mz.Width,mz.Height,Helpers.GeneratorEnum.Sets);
            maz.setupFromMaze(mz);
            await Shell.Current.Navigation.PushAsync(new MoveVizualizerPage(maz,whereIsMazeSaved)).ConfigureAwait(true);
        }
        [RelayCommand]
        public async Task GoToPlay()
        {
            GameMaze mz;
            if (local && ID >= 0)
            {
                mz = await MazeFetcher.getMazeLocalbyID(ID);
                mz.MazeID = ID;
            }
            else
            {
                mz = await MazeFetcher.getMaze(ID).ConfigureAwait(true);
            }
            if (mz.mazeType == MazeType.Classic)
            {
                Maze maz = new Maze(mz.Width, mz.Height, Helpers.GeneratorEnum.Sets);
                maz.setupFromMaze(mz);
                await Shell.Current.Navigation.PushAsync(new MazePage(maz)).ConfigureAwait(true);
            }
            else
            {
                await Shell.Current.Navigation.PushAsync(new RoundedMazePage((MauiMaze.Models.RoundedMaze.RoundedMaze)mz)).ConfigureAwait(true);
            }
        }
        [RelayCommand]
        public async Task deleteM(int id)
        {
            await MazeFetcher.deleteMazelocaly(id);
            await Shell.Current.Navigation.PopAsync();
            await Shell.Current.Navigation.PushAsync(new MazeHistoryPage());
        }
        [RelayCommand]
        public async Task saveOnline(int id)
        {
            GameMaze mz = await MazeFetcher.getMazeLocalbyID(id).ConfigureAwait(true);
            mz.MazeID = 0;
            int idd = UserDataProvider.GetInstance().getUserID();
            if (idd != -1)
            {
                MazeDTO mdto = new MazeDTO();
                mdto.startCell = mz.start.cell;
                mdto.endCell = mz.end.cell;
                mdto.size = mz.Width;
                mdto.edges = mz.Edges;
                await MazeFetcher.SaveMazeOnline(idd, mdto);
                await MazeFetcher.deleteMazelocaly(id);
                await Shell.Current.Navigation.PopAsync();
                await Shell.Current.Navigation.PushAsync(new MazeHistoryPage());
            }


        }

    }
}
