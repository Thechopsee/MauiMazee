using MauiMaze.ViewModels;

namespace MauiMaze;

public partial class UserListPage : ContentPage
{
	public UserListPage(UserListPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}