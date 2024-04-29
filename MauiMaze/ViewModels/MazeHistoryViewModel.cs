using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiMaze.Helpers;
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
        [ObservableProperty]
        bool loggedIn;
        [ObservableProperty]
        bool loading;
        [ObservableProperty]
        bool showLocal;
        public MazeHistoryViewModel()
        {
            if (UserDataProvider.GetInstance().getUserID() != -1)
            {
                LoggedIn = true;
            }
            if (UserDataProvider.GetInstance().getUserRole() == RoleEnum.Reseacher )
            {
                ShowLocal = false;
            }
            else
            {
                loadRecord();
                ShowLocal = true;
            }
            Loading = true;

        }
        public async void loadRecord()
        {
            if (UserDataProvider.GetInstance().getUserID() != -1)
            {
                Records = (await MazeFetcher.getMazeList(UserDataProvider.GetInstance().getUserID())).ToList();
            }
            (Mazes, Md) = await MazeFetcher.getOfflineMazes();
            Loading = false;
        }
        public async void loadRecordByuser(int id)
        {
            Records =(await MazeFetcher.getMazeList(id)).ToList();
        }

        [RelayCommand]
        public async Task deleteM(int id)
        {
            await MazeFetcher.deleteMazelocaly(id);
            await Shell.Current.Navigation.PushAsync(new MazeHistoryPage());
        }
    }
}
