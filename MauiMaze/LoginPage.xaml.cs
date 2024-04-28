using MauiMaze.Helpers;
using MauiMaze.ViewModels;

namespace MauiMaze;

public partial class LoginPage : ContentPage
{
	public LoginPage(RoleEnum role)
	{
		InitializeComponent();
		LoginPageViewModel lvm = new LoginPageViewModel();
		lvm.Role = role;
		BindingContext = lvm;
    }
}