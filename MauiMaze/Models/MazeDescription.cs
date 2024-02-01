using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiMaze.Models.ClassicMaze;
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

        public string description { get; set; }

        private bool local = false;

        public MazeDescription() { }
        public MazeDescription(int id,MazeType mt,DateTime cD) {
            this.ID = id;
            this.mazeType = mt;
            this.creationDate = cD;
            this.local = true;
            this.description = "" + ID + mt;
        }

        [RelayCommand]
        public async Task GoToMoves()
        {
            Maze mz;
            if (local && ID >= 0)
            {
                 mz= await MazeFetcher.getMazeLocalbyID(ID);
            }
            else
            {
                 mz = await MazeFetcher.getMaze(ID).ConfigureAwait(true);
            }
            await Shell.Current.Navigation.PushAsync(new MoveVizualizerPage(mz)).ConfigureAwait(true);
        }
        [RelayCommand]
        public async Task GoToPlay()
        {
            Maze mz;
            if (local && ID >= 0)
            {
                mz = await MazeFetcher.getMazeLocalbyID(ID);
            }
            else
            {
                mz = await MazeFetcher.getMaze(ID).ConfigureAwait(true);
            }
            await Shell.Current.Navigation.PushAsync(new MazePage(mz)).ConfigureAwait(true);
        }

    }
}
