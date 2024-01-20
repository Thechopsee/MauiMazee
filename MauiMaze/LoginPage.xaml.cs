using MauiMaze.ViewModels;

namespace MauiMaze;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
		LoginPageViewModel lvm = new LoginPageViewModel();
        lvm.Email = "admin";
        lvm.Password = "admin";
		BindingContext = lvm;
    }
}