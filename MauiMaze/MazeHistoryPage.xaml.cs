
using MauiMaze.Models;
using MauiMaze.Models.ClassicMaze;
using MauiMaze.Services;


namespace MauiMaze;

public partial class MazeHistoryPage : ContentPage
{
	public MazeHistoryPage()
	{
		InitializeComponent();
        downasda();
        
    }

    public async void downasda()
    {
        MazeDescription[] records = await MazeFetcher.getMazeList(UserDataProvider.GetInstance().getUserID());
        await Application.Current.MainPage.DisplayAlert("Upozornìní", "run " + records.Length, "OK");
        recordsList.ItemsSource = records.ToList();
    }

    private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        MazeDescription mazeDescription = e.SelectedItem as MazeDescription;
        Maze mz=await MazeFetcher.getMaze(mazeDescription.ID);
        await Navigation.PushAsync(new MazePage(mz));
    }
}