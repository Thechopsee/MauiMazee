using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiMaze.Models.DTOs;
using MauiMaze.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMaze.Models
{
    public partial class UserDataModel :ObservableObject
    {
        public int id { get; set; }
        public int role { get; set; }
        public string email { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public UserDataModel(UserDataDTO dto)
        {
            this.id = dto.id;
            this.role = dto.role;
            this.email = dto.email;
            this.firstname = dto.firstname;
            this.lastname = dto.lastname;
        }
        [RelayCommand]
        public async Task goToVizualizer()
        {
            MazeHistoryViewModel mv = new MazeHistoryViewModel();
            mv.loadRecordByuser(id);
            
            await Shell.Current.Navigation.PushAsync(new MazeHistoryPage(mv));
        }
    }
}
