using CommunityToolkit.Maui.Views;

namespace MauiMaze;

public partial class AreUSurePopUp : Popup
{
    public bool clicked { get; set; }
    public bool result { get; set; }
    public bool closed { get; set; }
    public AreUSurePopUp()
	{
        clicked = false;
        result = false;
        closed = false;
		InitializeComponent();
	}
    public AreUSurePopUp(String text)
    {
        clicked = false;
        result = false;
        closed = false;
        question.Text = text;
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