using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MauiMaze.Models
{
    public class DeviceInfo
    {
        public bool datavalidity { get; set; }
        public DeviceInfo()
        {
            try
            {
                getDeviceInfo();
                datavalidity = true;
            }
            catch
            {
                datavalidity = false;
            }
        }
        public void getDeviceInfo()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            /*
            sb.AppendLine($"Model: {DeviceInfo.Current.Model}");
            sb.AppendLine($"Manufacturer: {DeviceInfo.Current.Manufacturer}");
            sb.AppendLine($"Name: {DeviceInfo.Current.Name}");
            sb.AppendLine($"OS Version: {DeviceInfo.Current.VersionString}");
            sb.AppendLine($"Idiom: {DeviceInfo.Current.Idiom}");
            sb.AppendLine($"Platform: {DeviceInfo.Current.Platform}");

            bool isVirtual = DeviceInfo.Current.DeviceType switch
            {
                DeviceType.Physical => false,
                DeviceType.Virtual => true,
                _ => false
            };

            sb.AppendLine($"Virtual device? {isVirtual}");

            DisplayDeviceLabel.Text = sb.ToString();
            */
        }

    }
}
