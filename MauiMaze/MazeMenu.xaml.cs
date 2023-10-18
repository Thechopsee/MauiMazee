using MauiMaze.Drawables;
using MauiMaze.Models.ClassicMaze;
using MauiMaze.Services;
using MauiMaze.ViewModels;
using Newtonsoft.Json;

namespace MauiMaze;
public enum MazeType { 
    Classic,
    Rounded,
    Hexa
}
public partial class MazeMenu : ContentPage
{
    MazeDrawable mazeDrawable;
    Color native;
    Color clicked;
    MazeType actual;
    Maze maze;
    public MazeMenu()
	{
		InitializeComponent();
        mazeDrawable = this.Resources["MazeDrawable"] as MazeDrawable;
        maze=new Maze(new Size(10, 10));
        mazeDrawable.maze=maze;
        canvas.Invalidate();
        clicked = classicButton.BackgroundColor;
        native = hexaButton.BackgroundColor;
        actual = MazeType.Classic;
    }

    void OnSliderValueChanged(object sender, ValueChangedEventArgs args)
    {
        int size = Convert.ToInt32(((Slider)sender).Value);
        valueLabel.Text = size.ToString("F0");
        Maze maze = new Maze(new Size(size, size));
        mazeDrawable.maze=maze;
        canvas.Invalidate();
    }

    private async void mazeMenuNavigate(object sender, EventArgs e)
    {
        //await UserComunicator.tryToSaveMaze(UserDataProvider.GetInstance().getUserID(), maze.Edges);
        int size = Convert.ToInt32(sizeSlider.Value);
        switch (actual)
        {
            case MazeType.Classic:
                await Navigation.PushAsync(new MazePage(size));
                return;
            case MazeType.Rounded:
                await Navigation.PushAsync(new RoundedMazePage(25));
                return;
            case MazeType.Hexa:
                await Navigation.PushAsync(new HexagonalMazePage());
                return;
        }
        
    }
    private async void roundedMazeMenuNavigate(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RoundedMazePage(1));
    }

    private async void hexagonlMazeMenuNavigate(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new HexagonalMazePage());
    }

    private void hexaClick(object sender, EventArgs e)
    {
        actual = MazeType.Hexa;
        hexaButton.BackgroundColor = clicked;
        classicButton.BackgroundColor = native;
        roundedButton.BackgroundColor = native;
    }

    private void roundedClick(object sender, EventArgs e)
    {
        canvas.Drawable = new RoundedMazeDrawable();
        actual = MazeType.Rounded;
        hexaButton.BackgroundColor = native;
        classicButton.BackgroundColor = native;
        roundedButton.BackgroundColor = clicked;
    }

    private void classicClick(object sender, EventArgs e)
    {
        MazeDrawable md = new MazeDrawable();
        md.maze = new Maze(new Size(10, 10));
        canvas.Drawable = md;
        actual = MazeType.Classic;
        hexaButton.BackgroundColor = native;
        classicButton.BackgroundColor = clicked;
        roundedButton.BackgroundColor = native;
    }
}