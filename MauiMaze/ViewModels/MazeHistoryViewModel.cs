using MauiMaze.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMaze.ViewModels
{
    public class MazeHistoryViewModel : INotifyPropertyChanged
    {
        private MazeDescription mazeDescription;

        public MazeHistoryViewModel(MazeDescription ds)
        {
            mazeDescription = ds;
        }

        public MazeDescription NameRecord
        {
            get => mazeDescription;
            set
            {
                if (mazeDescription != value)
                {
                    mazeDescription = value;
                    OnPropertyChanged(nameof(MazeDescription));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
