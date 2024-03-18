using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiMaze.Helpers;
using MauiMaze.Models;
using MauiMaze.Models.DTOs;
using MauiMaze.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MauiMaze.ViewModels
{
    public partial class LoginPageViewModel : ObservableObject
    {
        [ObservableProperty]
        public string email;
        [ObservableProperty]
        public string password;
        [ObservableProperty]
        bool nameisValid;
        [ObservableProperty]
        bool offlineButton;
        [ObservableProperty]
        RoleEnum role;
        [RelayCommand]
        async Task goToRegister()
        {
            await Shell.Current.Navigation.PushAsync(new RegisterPage());
            

        }
        [RelayCommand]
        async Task tryToLoginn()
        {
            NameisValid = true;
            try
            {
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    bool vysl = await UserDataProvider.GetInstance().LoginUser(Email.Trim(), Password.Trim());
                    if (vysl)
                    {
                        
                        if (Role == RoleEnum.User)
                        {
                            await Shell.Current.Navigation.PushAsync(new UserMenu(LoginCases.Online));
                        }
                        else if(Role == RoleEnum.Reseacher) 
                        {
                            UserDataDTO[] users=await UserComunicator.getUsers();
                            UserListPageViewModel uvm = new UserListPageViewModel(users);
                            await Shell.Current.Navigation.PushAsync(new UserListPage(uvm));
                        }
                        Password = "";
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Chyba", "Špatné jméno nebo heslo", "OK");
                    }
                }
                else if (current == NetworkAccess.Local || current == NetworkAccess.None || current == NetworkAccess.Unknown)
                {
                    await Shell.Current.DisplayAlert("Chyba", "Nemáte přístup k internetu", "OK");
                    OfflineButton = true;
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Upozornění", "Zjistěna chyba při zjišťovaní stavu sítě", "OK");
                Console.WriteLine($"Chyba: {ex.Message}");
                OfflineButton = true;
            }

        }

    }
}
