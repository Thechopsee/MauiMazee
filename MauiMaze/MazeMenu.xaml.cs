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
}