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
    public partial class MainPageViewModel : ObservableObject
    {

        [RelayCommand]
        async Task GoOffline()
        {
            await Shell.Current.Navigation.PushAsync(new UserMenu(LoginCases.Offline));
        }
        [RelayCommand]
        async Task GoToSettings()
        {
            await Shell.Current.Navigation.PushAsync(new Settings());
        }
        [RelayCommand]
        async Task GoToScience()
        {
            await Shell.Current.Navigation.PushAsync(new LoginPage());
        }
        [RelayCommand]
        async Task GoToFreePlay()
        {
            await Shell.Current.Navigation.PushAsync(new UserMenu(LoginCases.Offline));
        }
        [RelayCommand]
        async Task GoToReport()
        {
            await Shell.Current.Navigation.PushAsync(new ReportBugPage()); 
        }
    }
}
