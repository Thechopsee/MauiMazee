using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
    public partial class UserListPageViewModel : ObservableObject
    {
        [ObservableProperty]
        public List<UserDataModel> users;
        [ObservableProperty]
        public List<VerificationCode> codes;
        public UserListPageViewModel(UserDataDTO[] users ) {
            this.Users = new List<UserDataModel>();
            this.Codes = new List<VerificationCode>();
            getList();
            foreach (var user in users )
            {
                this.Users.Add(new UserDataModel(user));
            }
        }
        private async Task getList()
        {
            Codes = (await VCFetcher.getUnusedCodes()).ToList();
        }
        [RelayCommand]
        public async Task generateCode()
        {
            for (int i = 0; i < 5; i++)
            {
                await VCFetcher.GenerateNewCodes();
            }
            await getList();
        }
    }
}
