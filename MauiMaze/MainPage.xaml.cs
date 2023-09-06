using MauiMaze.ViewModels;

namespace MauiMaze;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
        InitializeComponent();
        BindingContext = new MainPageViewModel();
    }

	private async void mazeMenuNavigate(object sender, EventArgs e)
	{
        await Navigation.PushAsync(new MazeMenu());
    }
    
}

