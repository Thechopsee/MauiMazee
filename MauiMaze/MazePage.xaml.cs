
using MauiMaze.Drawables;
using MauiMaze.Engine;

namespace MauiMaze;

public partial class MazePage : ContentPage
{
    MazeDrawable mazeDrawable;
    GameDriver driver;
    void GameView_DragInteraction(System.Object sender, Microsoft.Maui.Controls.TouchEventArgs e)
    {
        var touch = e.Touches.First();
        //Application.Current.MainPage.DisplayAlert("Upozornìní", "Touch" + e.Touches.Length, "OK");
        driver.movePlayerToPosition(touch.X, touch.Y);
        /*double deltaX = touch.X - touch.StartX;
        double deltaY = touch.Y - touch.StartY;

        if (Math.Abs(deltaX) > Math.Abs(deltaY)) // Posun v ose X je vìtší než v ose Y
        {
            if (deltaX > 0) // Posunul se doprava
            {
                // Provést akci pro posun doprava
            }
            else if (deltaX < 0) // Posunul se doleva
            {
                // Provést akci pro posun doleva
            }
        }
        else // Posun v ose Y je vìtší než v ose X
        {
            if (deltaY > 0) // Posunul se nahoru
            {
                // Provést akci pro posun nahoru
            }
            else if (deltaY < 0) // Posunul se dolù
            {
                // Provést akci pro posun dolù
            }
        }*/
    }
    public MazePage()
	{
		InitializeComponent();
        mazeDrawable = this.Resources["MazeDrawable"] as MazeDrawable;
        driver = new GameDriver(mazeDrawable,canvas);
    }


}