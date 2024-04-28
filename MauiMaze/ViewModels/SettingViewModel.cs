using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiMaze.Services;
using Microsoft.Maui.Layouts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMaze.ViewModels
{
    public partial class SettingViewModel : ObservableObject
    {
        public bool isloaded { get; set; }
        [ObservableProperty]
        SettingsData settingsData;
        [ObservableProperty]
        int valuee;
        [ObservableProperty]
        bool czechLanguage;
        [ObservableProperty]
        bool englishLanguage;
        [ObservableProperty]
        bool simplifyEnabled;
        [ObservableProperty]
        bool texturedEnabled;
        [ObservableProperty]
        string buttonSaveText;
        [RelayCommand]
        private void dragEnded()
        {
            SettingsData.test = Valuee;
        }
        [RelayCommand]
        private void saveSettings()
        {
           SettingsDataFetcher.saveSettings(SettingsData);
        }
        [RelayCommand]
        private void changeCzech()
        {
            SettingsData.language = "czechLanguage";
            ButtonSaveText = "Uložit nastavení";
            CzechLanguage = false;
            EnglishLanguage = true;
        }
        [RelayCommand]
        private void changeEnglish()
        {
            SettingsData.language = "englishLanguage";
            ButtonSaveText = "Save settings";
            CzechLanguage = true;
            EnglishLanguage = false;
        }
        [RelayCommand]
        public void goToReportPage()
        {
            Shell.Current.Navigation.PushAsync(new ReportBugPage());
        }
        [RelayCommand]
        public void switchGraphic()
        {
            if (SimplifyEnabled)
            {
                TexturedEnabled = true;
                SimplifyEnabled = false;
                SettingsData.SetsGenerator = true;
                saveSettings();
            }
            else if (TexturedEnabled)
            {
                TexturedEnabled = false;
                SimplifyEnabled = true;
                SettingsData.SetsGenerator = false;
                saveSettings();
            }
        }
        [RelayCommand]
        private async Task deleteData()
        {
            AreUSurePopUp areUSurePopUp = new("Delete all saved data?");
            var result = await Shell.Current.CurrentPage.ShowPopupAsync(areUSurePopUp);
            if ((bool)result)
            {
                int[] deletedIDs = await MazeFetcher.deleteAllMazes();
                for (int i = 0; i < deletedIDs.Length; i++)
                {
                    await RecordFetcher.deleteRecordsByMazeOffline(deletedIDs[i]);
                }
            }
            
        }
        public SettingViewModel() {
            CzechLanguage = false;
            EnglishLanguage = false;
            isloaded = false;
        }
        public async void tryToLoadSetting(object sender, EventArgs e)
        {
            SettingsData = await SettingsDataFetcher.getSettings();
            if (SettingsData.SetsGenerator)
            {
                SimplifyEnabled = false;
                TexturedEnabled = true;
            }
            else
            {
                SimplifyEnabled = true;
                TexturedEnabled = false;
            }
            if (String.Equals(SettingsData.language, "czechLanguage"))
            {
                ButtonSaveText = "Uložit nastavení";
                CzechLanguage = false;
                EnglishLanguage = true;
            }
            else
            {
                EnglishLanguage = false;
                CzechLanguage = true;
                ButtonSaveText = "Save settings";
            }
            isloaded = true;
            Valuee = SettingsData.test;
        }
    }
}
