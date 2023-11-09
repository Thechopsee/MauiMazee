using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMaze.Services
{
    class SettingsDataProvider
    {
        public static async Task<SettingsData> getSettings()
        {
            string data = await SecureStorage.Default.GetAsync("setting_json").ConfigureAwait(false);

            if (data == null)
            {
                return new SettingsData();
            }
            else
            {
                return JsonConvert.DeserializeObject<SettingsData>(data);
            }
            
        }
        public static async void saveSettings(SettingsData data)
        {
            await SecureStorage.Default.SetAsync("setting_json", JsonConvert.SerializeObject(data).ToString()).ConfigureAwait(false);
        }
    }
}
