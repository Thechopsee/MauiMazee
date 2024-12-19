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
            if (ServiceConfig.SMTP_name.Trim().Length==0 || ServiceConfig.SMTP_pass.Trim().Length==0)
            {
               Shell.Current.DisplayAlert("Error","Error:This feature not natively wokr on opensource code ,because SMTP server is not set in ServiceConfig.Update it with smtp name and password or use official release to continue","OK");
                return false;
            }
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(ServiceConfig.SMTP_name);
            mailMessage.To.Add("sebastian.walent@gmail.com");
            mailMessage.Subject = subject;
            DeviceData dd = new DeviceData();
            string devicedata = dd.dataString;
            mailMessage.Body ="Bug description: \n"+ text +"\n Sended from: \n"+sender+"\n Device data: \n"+devicedata;

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "smtp.mailersend.net";
            smtpClient.Port = 587;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(ServiceConfig.SMTP_name, ServiceConfig.SMTP_pass);
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
