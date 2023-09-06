using MauiMaze.Engine;
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
        await Navigation.PushAsync(new UserMenu());
    }
}