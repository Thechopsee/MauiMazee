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

        public RecordFullPageViewModel(GameRecord gr,int camefrom)
        {
            this.camefrom = camefrom;
            this.gameRecord = gr;
            foreach (int id in gr.cellPath)
            {
                CellPathString += "->" + id;
            }
        }


        [RelayCommand]
        private async Task backToMenu()
        {
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
