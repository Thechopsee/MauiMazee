using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiMaze.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MauiMaze.ViewModels
{
    public partial class RegisterPageViewModel : ObservableObject
    {
        [ObservableProperty]
        public string email;
        [ObservableProperty]
        public string password;
        [ObservableProperty]
        public string rePassword;
        [ObservableProperty]
        public string code;
        [ObservableProperty]
        public string first;
        [ObservableProperty]
        public string last;

        [ObservableProperty]
        public string errorMessage;

        [ObservableProperty]
        public bool loading;
        [RelayCommand]
        public async Task register()
        {
            Loading = true;
            bool failed = false;
            ErrorMessage = "";
            if (First is not null)
            {
                if (First.Trim().Length <= 0)
                {
                    ErrorMessage += "First name bad format\n";
                    failed = true;
                }
            }
            else
            {
                ErrorMessage += "First name is empty\n";
                failed = true;
            }
            if (Last is not null)
            {
                if (Last.Trim().Length <= 0)
                {
                    ErrorMessage += "Last name bad format\n";
                    failed = true;
                }
            }
            else
            {
                ErrorMessage += "Last name is empty\n";
                failed = true;
            }
            if (IsValidEmail() is false)
            {
                ErrorMessage += "Email has bad format\n";
                failed = true;
            }
            if (Password is not null)
            {
                if (Password != RePassword)
                {
                    ErrorMessage += "Passwords not match\n";
                    failed = true;
                }
            }
            else
            {
                ErrorMessage += "Password is null\n";
                failed = true;
            }
            if (Code is not null)
            {
                if (Code.Length != 8)
                {
                    ErrorMessage += "Bad Code";
                    failed = true;

                }
            }
            else
            {
                ErrorMessage += "Bad Code";
                failed = true;
            }
            if (!failed)
            {
               int status =await UserComunicator.tryToRegister(Email,Password,Code,First,Last);
                if (status == 1)
                {
                    ErrorMessage = "Wrong or Used Code";
                }
                else if (status == 2)
                {
                    ErrorMessage = "Server Error";
                }
                else
                {
                    await Shell.Current.DisplayAlert("OK", "Registration is OK \n Now try to login", "OK");
                    await Shell.Current.Navigation.PopToRootAsync();
                } 
            }
            Loading = false;
        }

         bool IsValidEmail()
        {
            return MailAddress.TryCreate(Email, out _);
        }

    }
}
