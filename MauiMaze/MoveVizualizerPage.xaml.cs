using MauiMaze.Drawables;
using MauiMaze.Engine;
using MauiMaze.Models;
using MauiMaze.Models.ClassicMaze;
using MauiMaze.ViewModels;

namespace MauiMaze;

public partial class MoveVizualizerPage : ContentPage
{
    MoveVizualizerViewModel mv;
	public MoveVizualizerPage(Maze maze, LoginCases lc)
	{
        
        InitializeComponent();
        MazeDrawable md = new MazeDrawable();
        md.maze = maze;
        canvas.Drawable = md;
        mv = new MoveVizualizerViewModel(canvas, recordsList, maze, lc, heatmap);
        BindingContext = mv;
        this.Loaded += refreshCanvas;
        
        


    }
    public  void refreshCanvas(object sender, EventArgs es)
    {
        canvas.Invalidate();
    }
    public void OnItemSelectedChanged(object sender, SelectedItemChangedEventArgs e)
    {
        
        mv.selectChanged((GameRecord)e.SelectedItem);
    }
}