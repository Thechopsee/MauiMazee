using MauiMaze.Models;
using MauiMaze.ViewModels;

namespace MauiMaze;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
        InitializeComponent();
        MainPageViewModel mpvm=new MainPageViewModel();

        BindingContext = mpvm;
    }

    
}

