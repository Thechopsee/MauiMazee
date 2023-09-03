using MauiMaze.Drawables;
using MauiMaze.Models.ClassicMaze;
using MauiMaze.ViewModels;

namespace MauiMaze;
public enum MazeTypeButtons { 
    Classic,
    Rounded,
    Hexa
}
public partial class MazeMenu : ContentPage
{
    MazeDrawable mazeDrawable;
    Color native;
    Color clicked;
    MazeTypeButtons actual;
    public MazeMenu()
	{
		InitializeComponent();
        mazeDrawable = this.Resources["MazeDrawable"] as MazeDrawable;
        Maze maze=new Maze(new Size(10, 10));
        mazeDrawable.maze=maze;
        canvas.Invalidate();
        clicked = classicButton.BackgroundColor;
        native = hexaButton.BackgroundColor;
        actual = MazeTypeButtons.Classic;
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
        int size = Convert.ToInt32(sizeSlider.Value);
        switch (actual)
        {
            case MazeTypeButtons.Classic:
                await Navigation.PushAsync(new MazePage(size));
                return;
            case MazeTypeButtons.Rounded:
                await Navigation.PushAsync(new RoundedMazePage(25));
                return;
            case MazeTypeButtons.Hexa:
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
        actual = MazeTypeButtons.Hexa;
        hexaButton.BackgroundColor = clicked;
        classicButton.BackgroundColor = native;
        roundedButton.BackgroundColor = native;
    }

    private void roundedClick(object sender, EventArgs e)
    {
        canvas.Drawable = new RoundedMazeDrawable();
        actual = MazeTypeButtons.Rounded;
        hexaButton.BackgroundColor = native;
        classicButton.BackgroundColor = native;
        roundedButton.BackgroundColor = clicked;
    }

    private void classicClick(object sender, EventArgs e)
    {
        MazeDrawable md = new MazeDrawable();
        md.maze = new Maze(new Size(10, 10),false);
        canvas.Drawable = md;
        actual = MazeTypeButtons.Classic;
        hexaButton.BackgroundColor = native;
        classicButton.BackgroundColor = clicked;
        roundedButton.BackgroundColor = native;
    }
}