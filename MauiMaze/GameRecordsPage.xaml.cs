
using MauiMaze.Engine;
using MauiMaze.Services;
using MauiMaze.ViewModels;

namespace MauiMaze;

public partial class GameRecordsPage : ContentPage
{
	public GameRecordsPage()
    {
        InitializeComponent();
        BindingContext = new GameRecordPageViewModel();
    }
}