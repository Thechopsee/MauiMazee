using MauiMaze.ViewModels;


namespace MauiMaze;

public partial class Settings : ContentPage
{
	public Settings()
	{
		InitializeComponent();
		SettingViewModel swm= new SettingViewModel();
		
		BindingContext = swm;
		
		swm.CzechLanguage = !swm.CzechLanguage;
        swm.CzechLanguage = !swm.CzechLanguage;
        swm.EnglishLanguage = !swm.EnglishLanguage;
        swm.EnglishLanguage = !swm.EnglishLanguage;

        InvalidateMeasure();
		loading.IsRunning = false;
	}
	
}