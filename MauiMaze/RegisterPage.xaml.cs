using MauiMaze.ViewModels;

namespace MauiMaze;

public partial class RegisterPage : ContentPage
{
	public RegisterPage()
	{
		InitializeComponent();
		BindingContext =new RegisterPageViewModel();
	}
}