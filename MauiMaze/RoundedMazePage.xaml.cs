using MauiMaze.Drawables;
using MauiMaze.Engine;
using MauiMaze.Models;

namespace MauiMaze;

public partial class RoundedMazePage : ContentPage
{
    BaseMazeDrawable mazeDrawable;
    GameDriver driver;
    bool hardend = false;
    //TODO SAVE PROCEDURE ON EXIT PASE ETC...
    async void GameView_DragInteraction(System.Object sender, Microsoft.Maui.Controls.TouchEventArgs e)
    {
        var touch = e.Touches.First();
        if (driver.ended)
        {
            await Navigation.PushAsync(new RecordFullPage(driver.gameRecord,0)).ConfigureAwait(false);
        }
        driver.movePlayerToPosition(touch.X, touch.Y);

    }
    public RoundedMazePage(int size, LoginCases login)
	{
		InitializeComponent();
        mazeDrawable = this.Resources["MazeDrawable"] as RoundedMazeDrawable;
        driver = new GameDriver(mazeDrawable, Canvas, size,1);
    }
}