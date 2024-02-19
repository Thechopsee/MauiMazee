
using CommunityToolkit.Maui.Views;
using MauiMaze.Drawables;
using MauiMaze.Engine;
using MauiMaze.Models.ClassicMaze;

using IImage = Microsoft.Maui.Graphics.IImage;
using MauiMaze.Services;
using MauiMaze.Models;
using MauiMaze.Popups;
using MauiMaze.Exceptions;
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
    Timer timer;
    bool firstMove = true;
    void GameView_DragInteraction(System.Object sender, Microsoft.Maui.Controls.TouchEventArgs e)
    {
        if (firstMove)
        {
            timer = new Timer(driver.timerMove, null, 0, 33);
            driver.startWatch();
            firstMove = false;
        }
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
    public MazePage(int size)
    {
        InitializeComponent();
        mazeDrawable = this.Resources["MazeDrawable"] as MazeDrawable;
        driver = new GameDriver(mazeDrawable, canvas, size, 0);
    }
    public MazePage(Maze maze) {
        if (maze is null)
        {
            throw new MazeNotLoadedExpectation("Maze is not loaded when try to inicialize gamedriver");
        }
        InitializeComponent();
        save_btn.IsVisible = false;
        mazeDrawable = this.Resources["MazeDrawable"] as MazeDrawable;
        driver = new GameDriver(mazeDrawable,maze,canvas);
        canvas.Invalidate();
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
                await MazeFetcher.saveMazeLocally((Maze)mazeDrawable.maze).ConfigureAwait(true);
                await Application.Current.MainPage.DisplayAlert("Upozornìní", "saved", "OK").ConfigureAwait(false);
            }
        }
    }
}