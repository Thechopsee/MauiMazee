using MauiMaze.Engine;
using MauiMaze.Models;
using MauiMaze.Services;
using MauiMaze.ViewModels;

namespace MauiMaze;

public partial class RecordFullPage : ContentPage
{
    public GameRecordViewModel ViewModel { get; } 
    public RecordFullPage(GameRecord record)
	{
		InitializeComponent();
        ViewModel = new GameRecordViewModel(record);
        Application.Current.MainPage.DisplayAlert("Upozornìní", "Touch"+ViewModel.NameRecord.name, "OK");
        BindingContext = ViewModel;
    }

    private async void backToMenu(object sender, EventArgs e)
    {
        if (UserDataProvider.GetInstance().isUserValid)
        {
            await Navigation.PushAsync(new UserMenu(LoginCases.Online));
        }
        else
        {
            await Navigation.PushAsync(new UserMenu(LoginCases.Offline));
        }
    }
}