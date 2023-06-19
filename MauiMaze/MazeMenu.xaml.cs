namespace MauiMaze;

public partial class MazeMenu : ContentPage
{
	public MazeMenu()
	{
		InitializeComponent();
	}

    void OnSliderValueChanged(object sender, ValueChangedEventArgs args)
    {
        valueLabel.Text = ((Slider)sender).Value.ToString("F3");
    }

    private async void mazeMenuNavigate(object sender, EventArgs e)
    {
       await Navigation.PushAsync(new MazePage());
    }
    private async void roundedMazeMenuNavigate(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RoundedMazePage());
    }
}