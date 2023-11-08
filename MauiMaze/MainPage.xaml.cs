using MauiMaze.Models;
using MauiMaze.ViewModels;

namespace MauiMaze;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
        InitializeComponent();
        MainPageViewModel mpvm=new MainPageViewModel();
        mpvm.Email = "admin";
        mpvm.Password = "admin";
        BindingContext = mpvm;
    }



	private async void mazeMenuNavigate(object sender, EventArgs e)
	{
        await Navigation.PushAsync(new MazeMenu(LoginCases.Offline));
    }
    
}

