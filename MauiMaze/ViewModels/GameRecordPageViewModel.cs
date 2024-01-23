using CommunityToolkit.Mvvm.ComponentModel;
using MauiMaze.Engine;
using MauiMaze.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMaze.ViewModels
{
    public partial class GameRecordPageViewModel : ObservableObject
    {
        [ObservableProperty]
        GameRecord[] records;
        public GameRecordPageViewModel() 
        {
            fetchData();
        }
        private async void fetchData()
        {
            Records = await RecordFetcher.loadRecordsByUser(UserDataProvider.GetInstance().getUserID());
        }
    }
}
