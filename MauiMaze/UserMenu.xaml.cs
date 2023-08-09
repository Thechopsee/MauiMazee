namespace MauiMaze;

public partial class UserMenu : ContentPage
{
	private int userID;
	public UserMenu(int userID)
	{
		this.userID = userID;
		InitializeComponent();
	}

	private async void GoToMaze(object sender, EventArgs e)
	{
        await Navigation.PushAsync(new MazeMenu());
    }

	private async void GoToRecords(object sender, EventArgs e)
	{
        await Navigation.PushAsync(new GameRecordsPage(userID));
    }
}