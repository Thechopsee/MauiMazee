using MauiMaze.ViewModels;


namespace MauiMaze;

public partial class Settings : ContentPage
{
	public Settings()
	{
        SettingViewModel swm = new SettingViewModel();
        this.Loaded += swm.tryToLoadSetting;
        InitializeComponent();
		
		
		BindingContext = swm;
		
		swm.SimplifyEnabled = !swm.SimplifyEnabled;
        swm.SimplifyEnabled = !swm.SimplifyEnabled;
        swm.TexturedEnabled = !swm.TexturedEnabled;
        swm.TexturedEnabled = !swm.TexturedEnabled;

        InvalidateMeasure();
		loading.IsRunning = false;
	}
	
}