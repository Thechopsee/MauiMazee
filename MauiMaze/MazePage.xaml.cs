
using MauiMaze.Engine;

namespace MauiMaze;

public partial class MazePage : ContentPage
{
    public UserInputManager userInputManager { get; set; }
    void OnGameViewEndInteraction(object sender, TouchEventArgs e)
    {
        userInputManager.FinishTouch();
        Application.Current.MainPage.DisplayAlert("Upozornìní", "Touch", "OK");
    }

    void OnGameViewStartInteraction(object sender, TouchEventArgs e)
    {
        Application.Current.MainPage.DisplayAlert("Upozornìní", "Touch", "OK");
        //userInputManager.HandleTouch(e.Touches.First().X, GameView.Width);
    }

    public MazePage()
	{
        userInputManager = new UserInputManager();
		InitializeComponent();
    }


}