using MauiMaze.Services;

namespace MauiMaze;

public partial class ReportBugPage : ContentPage
{
	public ReportBugPage()
	{
		InitializeComponent();
	}
    private void SubmitButton_Clicked(object sender, EventArgs e)
    {
        string senderEmail = senderEntry.Text;
        string subject = subjectEntry.Text;
        string text = textEditor.Text;
        submitBTn.IsEnabled = false;

        if (!IsValidEmail(senderEmail))
        {
            DisplayAlert("Error", "Invalid email format for Sender.", "OK");
            submitBTn.IsEnabled = true;
            return;
        }

        if (string.IsNullOrWhiteSpace(subject))
        {
            DisplayAlert("Error", "Subject cannot be empty.", "OK");
            submitBTn.IsEnabled = true;
            return;
        }

        if (string.IsNullOrWhiteSpace(text))
        {
            DisplayAlert("Error", "Text cannot be empty.", "OK");
            submitBTn.IsEnabled = true;
            return;
        }


       bool status= EmailSender.SendEmail(subjectEntry.Text, textEditor.Text, senderEntry.Text);
        if (status)
        {
            Shell.Current.Navigation.PopAsync();
        }
    }
    private bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

}