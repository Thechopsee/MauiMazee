﻿namespace MauiMaze;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
	}

	private async void mazeMenuNavigate(object sender, EventArgs e)
	{
        await Navigation.PushAsync(new MazeMenu());
    }
}

