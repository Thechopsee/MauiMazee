using MauiMaze.Drawables;
using MauiMaze.Models;

namespace MauiMaze;

public partial class MazeMenu : ContentPage
{
    MazeDrawable mazeDrawable;
    public MazeMenu()
	{
		InitializeComponent();
        mazeDrawable = this.Resources["MazeDrawable"] as MazeDrawable;
        Maze maze=new Maze(new Size(10, 10));
        mazeDrawable.setNewMaze(maze);
        canvas.Invalidate();
    }

    void OnSliderValueChanged(object sender, ValueChangedEventArgs args)
    {
        int size = Convert.ToInt32(((Slider)sender).Value);
        valueLabel.Text = size.ToString("F0");
        Maze maze = new Maze(new Size(size, size));
        mazeDrawable.setNewMaze(maze);
        canvas.Invalidate();
    }

    private async void mazeMenuNavigate(object sender, EventArgs e)
    {
        int size = Convert.ToInt32(sizeSlider.Value);
        await Navigation.PushAsync(new MazePage(size));
    }
    private async void roundedMazeMenuNavigate(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RoundedMazePage());
    }
}