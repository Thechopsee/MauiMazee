
using MauiMaze.Models;
using MauiMaze.Models.ClassicMaze;
using MauiMaze.Services;
using MauiMaze.ViewModels;

namespace MauiMaze;

public partial class MazeHistoryPage : ContentPage
{
    MazeHistoryViewModel mhvm;

    public MazeHistoryPage()
	{
		InitializeComponent();
        BindingContext = new MazeHistoryViewModel();

    }
    public MazeHistoryPage(MazeHistoryViewModel mh)
    {
        InitializeComponent();
        BindingContext = mh;

    }



}