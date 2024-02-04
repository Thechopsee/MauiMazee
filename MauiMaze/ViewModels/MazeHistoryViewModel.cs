using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiMaze.Models;
using MauiMaze.Models.ClassicMaze;
using MauiMaze.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MauiMaze.ViewModels
{
    public partial class MazeHistoryViewModel : ObservableObject
    {
        [ObservableProperty]
        public List<MazeDescription> records;
        [ObservableProperty]
        public Maze[] mazes;
        [ObservableProperty]
        MazeDescription[] md;
        public ActivityIndicator ai;

        public MazeHistoryViewModel(ActivityIndicator ai)
        {
            this.ai = ai;
            loadRecord();
           
        }
        public async void loadRecord()
        {
            if (UserDataProvider.GetInstance().getUserID() != -1)
            {
                Records = await MazeProvider.Instance.loadMazes().ConfigureAwait(true);
            }  
            (Mazes,Md) = await MazeProvider.Instance.loadLocalMazes();
            ai.IsRunning = false;
        }



    }
}
