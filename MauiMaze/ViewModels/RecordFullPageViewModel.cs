using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiMaze.Engine;
using MauiMaze.Models;
using MauiMaze.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMaze.ViewModels
{
    public partial class RecordFullPageViewModel: ObservableObject
    {
        [ObservableProperty]
        private GameRecord gameRecord;
        private int camefrom;
        [ObservableProperty]
        public string cellPathString="0";
        [ObservableProperty]
        public string mazeIDLabel;


        public RecordFullPageViewModel(GameRecord gr,int camefrom)
        {
            this.camefrom = camefrom;
            this.gameRecord = gr;
            foreach (int id in gr.cellPath)
            {
                CellPathString += "->" + id;
            }
            if (gameRecord.mazeID == -1)
            {
                mazeIDLabel = "Random";
            }
            else
            {
                mazeIDLabel = ""+gameRecord.mazeID;
            }
        }
        private async Task saveMaze(GameRecord gr)
        {
            if (gr.userID != -1)
            {
                if (gr.mazeID != -1)
                {
                   await  RecordFetcher.SaveRecordOnline(gr.GetRecordDTO());
                }
            }
            else if(gr.mazeID > -1) 
            {

               await RecordFetcher.saveRecordByMazeOffline(gr);

            }
        }

        [RelayCommand]
        private async Task backToMenu()
        {
            await saveMaze(GameRecord);
            if (UserDataProvider.GetInstance().isUserValid)
            {
                await Shell.Current.Navigation.PushAsync(new UserMenu(LoginCases.Online));
            }
            else
            {
                await Shell.Current.Navigation.PushAsync(new UserMenu(LoginCases.Offline));
            }
        }
    }
}
