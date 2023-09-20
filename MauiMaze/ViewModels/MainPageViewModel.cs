﻿using CommunityToolkit.Mvvm.ComponentModel;
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
        [ObservableProperty]
        public string email;
        [ObservableProperty]
        public string password;
        [ObservableProperty]
        bool isBusy;
        bool isNotBusy => !IsBusy;
        [ObservableProperty]
        bool nameisValid;
        [ObservableProperty]
        bool offlineButton;
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
        async Task tryToLoginn()
        {
            NameisValid = true;
            IsBusy = true;
            try
            {
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    bool vysl = await UserDataProvider.GetInstance().LoginUser(Email.Trim(), Password.Trim());
                    if (vysl)
                    {
                        Password = "";
                        await Shell.Current.Navigation.PushAsync(new UserMenu(LoginCases.Online));
                        
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Chyba", "Špatné jméno nebo heslo", "OK");
                    }
                }
                else if (current == NetworkAccess.Local || current == NetworkAccess.None || current==NetworkAccess.Unknown)
                {
                    //TODO pada Interrnet nebo Unknown nic jiného asi vlastnost maui upravit
                    // Nemáte přístup k internetu
                    await Shell.Current.DisplayAlert("Chyba", "Nemáte přístup k internetu", "OK");
                    OfflineButton = true;
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Upozornění", "Zjistěna chyba při zjišťovaní stavu sítě", "OK");
                Console.WriteLine($"Chyba: {ex.Message}");
            }

            isBusy = false;

        }

        private bool checkValidity()
        {
            if (email == "admin")
            {
                return true;
            } else { return false; }
        }
    }
}