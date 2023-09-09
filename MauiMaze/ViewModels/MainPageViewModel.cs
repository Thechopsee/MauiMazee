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
    public partial class MainPageViewModel : ObservableObject
    {
        [ObservableProperty]
        public string email;
        [ObservableProperty]
        public string password;
        [ObservableProperty]
        bool isBusy;
        bool isNotBusy => !isBusy;
        [ObservableProperty]
        bool nameisValid;
        [RelayCommand]
        async Task tryToLoginn()
        {
            NameisValid = true;
            isBusy = true;
            try
            {
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    bool vysl = await UserDataProvider.GetInstance().LoginUser(Email, Password);
                    if (vysl)
                    {
                        //await Navigation.PushAsync(new UserMenu(userID));
                        Password = "";
                        await Shell.Current.Navigation.PushAsync(new UserMenu());
                        
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Chyba", "Špatné jméno nebo heslo", "OK");
                    }
                }
                else if (current == NetworkAccess.Local || current == NetworkAccess.None)
                {
                    // Nemáte přístup k internetu
                    Console.WriteLine("Nemáte přístup k internetu.");
                }
            }
            catch (Exception ex)
            {
                // Něco se pokazilo při zjišťování stavu sítě
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
