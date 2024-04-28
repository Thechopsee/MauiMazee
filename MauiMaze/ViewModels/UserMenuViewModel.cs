using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiMaze.Models;
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
        public bool isHistoryButtonVisible = true;
        [ObservableProperty]
        public bool isRecordsButtonVisible = true;
        [ObservableProperty]
        public bool isDailyButtonVisible = true;
        [ObservableProperty]
        public string exitText = "Logout 🚪";
        [ObservableProperty]
        public string welcomeText = "Welcome User!";
        LoginCases login;
        public UserMenuViewModel(LoginCases login)
        {
            this.login = login;
            if (login == LoginCases.Offline)
            {
                UserOfflineProcedure();
            }
        }
        public void UserOfflineProcedure()
        {
            IsRecordsButtonVisible = false;
            IsDailyButtonVisible = false;
            ExitText = "Exit 🚪";
        }
        public void SetWelcomeText(string name) => WelcomeText = $"Welcome {name}!";
        [RelayCommand]
        async Task GoToMaze()
        {
            await Shell.Current.Navigation.PushAsync(new MazeMenu(login));
        }
        [RelayCommand]
         static async Task GoToRecords()
        {
            await Shell.Current.Navigation.PushAsync(new GameRecordsPage(UserDataProvider.GetInstance().getUserID()));
        }
        [RelayCommand]
        static async Task Logout()
        {
            UserDataProvider.GetInstance().LogoutUser();
            await Shell.Current.Navigation.PopToRootAsync();
        }
        [RelayCommand]
        static async Task GoToHistory()
        {
            await Shell.Current.Navigation.PushAsync(new MazeHistoryPage());
        }
        [RelayCommand]
        static async Task GoToSettings()
        {
            await Shell.Current.Navigation.PushAsync(new Settings());
        }

    }
}
