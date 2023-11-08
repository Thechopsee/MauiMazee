using CommunityToolkit.Mvvm.ComponentModel;
using MauiMaze.Drawables;
using MauiMaze.Models;
using MauiMaze.Models.ClassicMaze;
using MauiMaze.Services;
using MauiMaze.ViewModels;
using Newtonsoft.Json;

namespace MauiMaze;

public partial class MazeMenu : ContentPage
{
    public MazeMenu(LoginCases login)
	{
		InitializeComponent();
        BindingContext = new MazeMenuViewModel(canvas,classicButton,roundedButton,login);
    }




}