using CommunityToolkit.Maui.Views;
using MauiMaze.Drawables;
using MauiMaze.Engine;
using MauiMaze.Helpers;
using MauiMaze.Models;
using MauiMaze.Models.RoundedMaze;
using MauiMaze.Popups;
using MauiMaze.Services;

namespace MauiMaze;

public partial class RoundedMazePage : ContentPage
{
    BaseMazeDrawable mazeDrawable;
    GameDriver driver;
    Timer timer;

    void GameView_DragInteraction(System.Object sender, Microsoft.Maui.Controls.TouchEventArgs e)
    {


        if (driver.ended)
        {
            Navigation.PushAsync(new RecordFullPage(driver.gameRecord, 0));
        }
        if (e is not null)
        {
            var touch = e.Touches.First();
            driver.setPosition(touch.X, touch.Y);
        }
    }

    private async void GoBackPop(object sender, EventArgs e)
    {
        AreUSurePopUp areUSurePopUp = new();
        var result = await this.ShowPopupAsync(areUSurePopUp);
        if (result is not null)
        {
            if ((bool)result)
            {
                await Navigation.PopAsync().ConfigureAwait(false);
            }
        }

    }

    private async void SaveMaze(object sender, EventArgs e)
    {
        ((Button)sender).IsVisible = false;
        SaveMazePopUp areUSurePopUp = new();
        var result = await this.ShowPopupAsync(areUSurePopUp);
        if (result is not null)
        {
            if ((bool)result)
            {
                await MazeFetcher.saveMazeLocally(mazeDrawable.maze).ConfigureAwait(true);
                await Application.Current.MainPage.DisplayAlert("Upozornìní", "saved", "OK").ConfigureAwait(false);
            }
        }
    }

    public RoundedMazePage(int size,GeneratorEnum gen)
	{
		InitializeComponent();
        
        mazeDrawable = this.Resources["MazeDrawable"] as RoundedMazeDrawable;
        driver = new GameDriver(mazeDrawable, Canvas, size,1,gen);
        timer = new Timer(driver.timerMove, null, 0, 33);
        Canvas.Invalidate();
    }
    public RoundedMazePage(RoundedMaze roundedMaze)
    {
        InitializeComponent();

        mazeDrawable = this.Resources["MazeDrawable"] as RoundedMazeDrawable;
        mazeDrawable.maze = roundedMaze;
        driver = new GameDriver(mazeDrawable, roundedMaze, Canvas);
        timer = new Timer(driver.timerMove, null, 0, 33);
    }
}