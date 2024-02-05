using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiMaze.Services;
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
        string buttonSaveText;
        [RelayCommand]
        private void dragEnded()
        {
            SettingsData.test = Valuee;
        }
        [RelayCommand]
        private void saveSettings()
        {
           SettingsDataProvider.saveSettings(SettingsData);
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
        private void deleteData()
        {
            //TODO
        }
        public SettingViewModel() {
            CzechLanguage = false;
            EnglishLanguage = false;
            isloaded = false;
            tryToLoadSetting();
        }
        public async void tryToLoadSetting()
        {
            SettingsData = await SettingsDataProvider.getSettings();
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
