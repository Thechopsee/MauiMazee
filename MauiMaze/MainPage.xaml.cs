namespace MauiMaze;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
	}

	private async void mazeMenuNavigate(object sender, EventArgs e)
	{
        await Navigation.PushAsync(new MazeMenu());
    }

	private async void tryToLogin(object sender, EventArgs e)
	{
        try
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                // Máte přístup k internetu
                Console.WriteLine("Máte přístup k internetu.");
                string email = EmailEntry.Text;
                string password = PassEntry.Text;
                int userID = 1; //TODO: získat ID uživatele z databáze
                if (email == "admin" && password == "admin")
                {
                    await Navigation.PushAsync(new UserMenu(userID));
                }
                else
                {
                    await DisplayAlert("Chyba", "Špatné jméno nebo heslo", "OK");
                }   
            }
            else if (current == NetworkAccess.Local || current == NetworkAccess.None)
            {
                // Nemáte přístup k internetu
                Console.WriteLine("Nemáte přístup k internetu.");
            }
        }
        catch (Exception ex)
        {
            // Něco se pokazilo při zjišťování stavu sítě
            Console.WriteLine($"Chyba: {ex.Message}");
        }
        

        
    }	 
}

