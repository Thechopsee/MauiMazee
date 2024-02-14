using CommunityToolkit.Mvvm.ComponentModel;
using MauiMaze.Models;
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
        public UserListPageViewModel(UserDataDTO[] users ) {
            this.Users = new List<UserDataModel>();
            foreach(var user in users )
            {
                this.Users.Add(new UserDataModel(user));
            }

        }
    }
}
