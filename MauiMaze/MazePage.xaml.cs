
using CommunityToolkit.Maui.Views;
using MauiMaze.Drawables;
using MauiMaze.Engine;
using MauiMaze.Models.ClassicMaze;

using IImage = Microsoft.Maui.Graphics.IImage;
using MauiMaze.Services;
using MauiMaze.Models;
using MauiMaze.Popups;
#if IOS || ANDROID || MACCATALYST
using Microsoft.Maui.Graphics.Platform;
#elif WINDOWS
using Microsoft.Maui.Graphics.Win2D;
#endif

namespace MauiMaze;

public partial class MazePage : ContentPage
{
    MazeDrawable mazeDrawable;
    GameDriver driver;
    LoginCases login;
    Timer timer;
    void GameView_DragInteraction(System.Object sender, Microsoft.Maui.Controls.TouchEventArgs e)
    {
        if (driver.ended)
        {
            Navigation.PushAsync(new RecordFullPage(driver.gameRecord, 0));
        }
        var touch = e.Touches.First();
        driver.movePlayerToPosition(touch.X, touch.Y);
    }
    public MazePage(int size, LoginCases login)
    {
        InitializeComponent();
        this.login = login;
        mazeDrawable = this.Resources["MazeDrawable"] as MazeDrawable;
        driver = new GameDriver(mazeDrawable, canvas, size, 0, login);
    }
    public MazePage(Maze maze) { 
        InitializeComponent();
        mazeDrawable = this.Resources["MazeDrawable"] as MazeDrawable;
        driver = new GameDriver(mazeDrawable, canvas, (int)(maze.Size.Width), 0,maze); //todo doupravit
    }

    private async void GoBackPop(object sender, EventArgs e)
    {
        AreUSurePopUp areUSurePopUp = new();
        var result = await this.ShowPopupAsync(areUSurePopUp).ConfigureAwait(false);
        if (result is not null)
        {
            if ((bool)result)
            {
                RecordRepository.GetInstance().addRecord(driver.gameRecord);
                await Navigation.PopAsync().ConfigureAwait(false);
            }
        }
        
    }

    private async void SaveMaze(object sender, EventArgs e)
    {
        SaveMazePopUp areUSurePopUp = new();
        var result = await this.ShowPopupAsync(areUSurePopUp).ConfigureAwait(false);
        if (result is not null)
        {
            if ((bool)result)
            {
                await MazeFetcher.saveMazeLocally((Maze)mazeDrawable.maze).ConfigureAwait(false);
                await Application.Current.MainPage.DisplayAlert("Upozornìní", "saved", "OK").ConfigureAwait(false);
            }
            else
            {
                Maze maze = (Maze)mazeDrawable.maze;
                await MazeFetcher.SaveMazeOnline(UserDataProvider.GetInstance().getUserID(), maze.Edges).ConfigureAwait(false);
                await Application.Current.MainPage.DisplayAlert("Upozornìní", "saved", "OK").ConfigureAwait(false);
            }
        }
    }
}