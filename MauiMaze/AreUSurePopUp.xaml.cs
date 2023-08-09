using CommunityToolkit.Maui.Views;

namespace MauiMaze;

public partial class AreUSurePopUp : Popup
{
    public bool clicked = false;
    public bool result = false;
    public bool closed = false;
	public AreUSurePopUp()
	{
		InitializeComponent();
	}

    private void setTrue(object sender, EventArgs e)
    {
        clicked = true;
        result = true;

        this.Close(true);
    }

    private void setFalse(object sender, EventArgs e)
    {
        clicked = true;
       
        this.Close(false);
    }
}