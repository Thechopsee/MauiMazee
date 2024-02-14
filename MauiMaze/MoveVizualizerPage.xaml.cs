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
        this.Loaded += refreshCanvas;
        mv = new MoveVizualizerViewModel(canvas, recordsList, maze,lc,heatmap);
        BindingContext = mv;
        


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