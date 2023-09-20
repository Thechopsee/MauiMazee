using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiMaze.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMaze.ViewModels
{
    public partial class UserMenuViewModel : ObservableObject
    {
        [ObservableProperty]
        public bool isHistoryButtonVisible=true;
        [ObservableProperty]
        public bool isRecordsButtonVisible=true;
        [ObservableProperty]
        public string exitText = "Logout 🚪";
        [ObservableProperty]
        public string welcomeText = "Welcome User!";
        public void UserOfflineProcedure()
        {
            IsHistoryButtonVisible = false;
            IsRecordsButtonVisible = false;
            ExitText = "Exit 🚪";
        }
        public void setWelcomeText(string name)
        {
            WelcomeText = $"Welcome {name}!";
        }
        [RelayCommand]
        async Task GoToMaze()
        {
            await Shell.Current.Navigation.PushAsync(new MazeMenu());
        }
        [RelayCommand]
         async Task GoToRecords()
        {
            await Shell.Current.Navigation.PushAsync(new GameRecordsPage(UserDataProvider.GetInstance().getUserID()));
        }
        [RelayCommand]
        async Task Logout()
        {
            UserDataProvider.GetInstance().LogoutUser();
            await Shell.Current.Navigation.PopToRootAsync();
        }
        [RelayCommand]
        async Task GoToHistory()
        {
            await Shell.Current.Navigation.PushAsync(new MazeHistoryPage());
        }
        [RelayCommand]
        async Task GoToSettings()
        {
            await Shell.Current.Navigation.PushAsync(new Settings());
        }

    }
}
