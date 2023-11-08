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

        [RelayCommand]
        public async Task GoToMoves()
        {
            if (ID > 0)
            {
                Maze mz = await MazeFetcher.getMaze(ID);
                await Shell.Current.Navigation.PushAsync(new MoveVizualizerPage(mz));
            }
        }
        [RelayCommand]
        public async Task GoToPlay()
        {
            if (ID > 0)
            {
                Maze mz = await MazeFetcher.getMaze(ID);
                await Shell.Current.Navigation.PushAsync(new MazePage(mz));
            }
        }

    }
}
