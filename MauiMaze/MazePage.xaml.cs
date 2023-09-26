
using CommunityToolkit.Maui.Views;
using MauiMaze.Drawables;
using MauiMaze.Engine;
using MauiMaze.Models.ClassicMaze;

using IImage = Microsoft.Maui.Graphics.IImage;
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

    //TODO SAVE PROCEDURE ON EXIT PASE ETC...
     void GameView_DragInteraction(System.Object sender, Microsoft.Maui.Controls.TouchEventArgs e)
    {
        var touch = e.Touches.First();
        //Application.Current.MainPage.DisplayAlert("Upozornìní", "Touch" + e.Touches.Length, "OK");
        if (driver.ended)
        {
             Navigation.PushAsync(new RecordFullPage(driver.gameRecord));
        }
            driver.movePlayerToPosition(touch.X, touch.Y);
    }
    public MazePage(int size)
	{
		InitializeComponent();
        mazeDrawable = this.Resources["MazeDrawable"] as MazeDrawable;
        driver = new GameDriver(mazeDrawable,canvas,size,0);
    }
    public MazePage(Maze maze)
    {
        InitializeComponent();
        mazeDrawable = this.Resources["MazeDrawable"] as MazeDrawable;
        driver = new GameDriver(mazeDrawable, canvas, (int)(maze.Size.Width), 0,maze); //todo doupravit
    }

    private async void GoBackPop(object sender, EventArgs e)
    {
        AreUSurePopUp areUSurePopUp = new();
        var result = await this.ShowPopupAsync(areUSurePopUp);
        if ((bool)result)
        {
            //TODO :Save procedure
            await Navigation.PopAsync();
        }
        
    }
}