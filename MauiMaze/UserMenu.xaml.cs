using MauiMaze.Services;

namespace MauiMaze;

public partial class UserMenu : ContentPage
{
	private int userID;
	public UserMenu()
	{
		InitializeComponent();
		if (UserDataProvider.GetInstance().isUserValid)
		{
			welcomelabel.Text = $"Welcome {UserDataProvider.GetInstance().getUserName()}!";
		}
	}

	private async void GoToMaze(object sender, EventArgs e)
	{
        await Navigation.PushAsync(new MazeMenu());
    }

	private async void GoToRecords(object sender, EventArgs e)
	{
        await Navigation.PushAsync(new GameRecordsPage(userID));
    }

    private async void Logout(object sender, EventArgs e)
    {
		UserDataProvider.GetInstance().LogoutUser();
		await Navigation.PopToRootAsync();
    }
}