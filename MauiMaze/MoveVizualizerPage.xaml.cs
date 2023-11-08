using MauiMaze.Engine;
using MauiMaze.Models.ClassicMaze;
using MauiMaze.ViewModels;

namespace MauiMaze;

public partial class MoveVizualizerPage : ContentPage
{
    MoveVizualizerViewModel mv;
	public MoveVizualizerPage(Maze maze)
	{
        InitializeComponent();
        mv= new MoveVizualizerViewModel(canvas, recordsList, maze);
        BindingContext = mv;
		
	}
    public void OnItemSelectedChanged(object sender, SelectedItemChangedEventArgs e)
    {
        
        mv.selectChanged((GameRecord)e.SelectedItem);
    }
}