using MauiMaze.Drawables;
using MauiMaze.Engine;

namespace MauiMaze.ViewModels;

public partial class HexagonalMazePage : ContentPage
{
    HexagonalMazeDrawable mazeDrawable;
    GameDriver driver;
    public HexagonalMazePage()
	{
		InitializeComponent();
 
        
    }
}