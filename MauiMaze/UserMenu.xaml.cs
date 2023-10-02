using MauiMaze.Models;
using MauiMaze.Services;
using MauiMaze.ViewModels;

namespace MauiMaze;

public partial class UserMenu : ContentPage
{
	public UserMenu(LoginCases login)
	{
		InitializeComponent();
		UserMenuViewModel view= new UserMenuViewModel();
        if (login != LoginCases.Offline)
        {
            view.SetWelcomeText(UserDataProvider.GetInstance().getUserName());
        }
        else {

            view.UserOfflineProcedure();
        }
        BindingContext = view;
	}
}