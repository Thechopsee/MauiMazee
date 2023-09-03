
using CommunityToolkit.Maui.Views;
using MauiMaze.Drawables;
using MauiMaze.Engine;

namespace MauiMaze;

public partial class MazePage : ContentPage
{
    MazeDrawable mazeDrawable;
    GameDriver driver;
    bool hardend = false;
    //TODO SAVE PROCEDURE ON EXIT PASE ETC...
    async void GameView_DragInteraction(System.Object sender, Microsoft.Maui.Controls.TouchEventArgs e)
    {
        var touch = e.Touches.First();
        //Application.Current.MainPage.DisplayAlert("Upozornění", "Touch" + e.Touches.Length, "OK");
        if (driver.ended)
        {
            await Navigation.PushAsync(new RecordFullPage(driver.gameRecord));
        }
        driver.movePlayerToPosition(touch.X, touch.Y);
       
    }
    public MazePage(int size)
	{
		InitializeComponent();
        mazeDrawable = this.Resources["MazeDrawable"] as MazeDrawable;
        driver = new GameDriver(mazeDrawable,canvas,size,0);
    }

    private async void goBackPop(object sender, EventArgs e)
    {
        AreUSurePopUp areUSurePopUp = new AreUSurePopUp();
        var result = await this.ShowPopupAsync(areUSurePopUp);
        if ((bool)result)
        {
            //TODO :Save procedure
            await Navigation.PopAsync();
        }
        
    }
}