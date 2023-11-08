using CommunityToolkit.Maui.Views;

namespace MauiMaze.Popups;

public partial class SaveMazePopUp : Popup
{
	public SaveMazePopUp()
	{
		InitializeComponent();
	}
    //Local
    private void setTrue(object sender, EventArgs e)
    {
        this.Close(true);
    }
    //Cloud
    private void setFalse(object sender, EventArgs e)
    {
        this.Close(false);
    }
}