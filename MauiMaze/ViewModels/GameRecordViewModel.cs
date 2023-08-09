using MauiMaze.Engine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMaze.ViewModels
{
    public class GameRecordViewModel : INotifyPropertyChanged
    {
        private GameRecord gameRecord;

        public GameRecordViewModel(GameRecord gr)
        {
            // Initialize the GameRecord instance or obtain it from a data source
            this.gameRecord = gr;
        }

        public GameRecord NameRecord
        {
            get => gameRecord;
            set
            {
                if (gameRecord != value)
                {
                    gameRecord = value;
                    OnPropertyChanged(nameof(GameRecord));
                }
            }
        }

        // Implement INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Add additional logic for setting properties, if required
    }
}
