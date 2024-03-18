using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using MauiMaze.Models;

namespace MauiMaze.Services
{
    public static class EmailSender
    {
        public static bool SendEmail(string subject,string text,string sender)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("MS_owVYCj@trial-jy7zpl99kmol5vx6.mlsender.net");
            mailMessage.To.Add("sebastian.walent@gmail.com");
            mailMessage.Subject = subject;
            DeviceData dd = new DeviceData();
            string devicedata = dd.dataString;
            mailMessage.Body ="Bug description: \n"+ text +"\n Sended from: \n"+sender+"\n Device data: \n"+devicedata;

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "smtp.mailersend.net";
            smtpClient.Port = 587;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential("MS_owVYCj@trial-jy7zpl99kmol5vx6.mlsender.net", "dcZMMwgaTfqMRX5d");
            smtpClient.EnableSsl = true;

            try
            {
                smtpClient.Send(mailMessage);
                Shell.Current.DisplayAlert("Send", "Email was send", "OK");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                Shell.Current.DisplayAlert("Error", "Error "+ex.Message, "OK");
                return false;
            }
        }
    }
}
