
using MauiMaze.Engine;
using MauiMaze.Services;
using MauiMaze.ViewModels;

namespace MauiMaze;

public partial class GameRecordsPage : ContentPage
{
	private int userID;
	public GameRecordsPage(int userID)
    {
        InitializeComponent();
        BindingContext = new GameRecordPageViewModel();
        this.userID = userID;
    }


}