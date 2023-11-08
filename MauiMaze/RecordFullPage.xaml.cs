using MauiMaze.Engine;
using MauiMaze.Models;
using MauiMaze.Services;
using MauiMaze.ViewModels;

namespace MauiMaze;

public partial class RecordFullPage : ContentPage
{
    public ViewModels.RecordFullPageViewModel ViewModel { get; } 
    public RecordFullPage(GameRecord record,int camefrom)
	{
		InitializeComponent();
        ViewModel = new ViewModels.RecordFullPageViewModel(record, camefrom);
        BindingContext = ViewModel;
    }


}