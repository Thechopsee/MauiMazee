using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.LocalNotification;

namespace MauiMaze.Models.Notification
{
    class GlobalNotificationManager
    {
        public static void TestNotification()
        {
            var request = new NotificationRequest
            {
                NotificationId = 1000,
                Title = "Subscribe for me",
                Subtitle = "Hello Friends",
                Description = "Stay Tuned",
                BadgeNumber = 42,
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = DateTime.Now.AddSeconds(5),
                    NotifyRepeatInterval = TimeSpan.FromDays(1)
                }
            };
            LocalNotificationCenter.Current.Show(request);
        }
    }
}
