using CommunityToolkit.Maui.Views;

namespace MauiMaze.Popups;

public partial class EndgamePopUp : Popup
{
	public EndgamePopUp()
	{
		InitializeComponent();
	}
    private void setTrue(object sender, EventArgs e)
    {
        this.Close(true);
    }
}