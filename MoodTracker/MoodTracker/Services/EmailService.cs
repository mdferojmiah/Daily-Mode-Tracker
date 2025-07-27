using System.Net;
using System.Net.Mail;
using MoodTracker.ServiceContracts;

namespace MoodTracker.Services;

public class EmailService: IEmailService
{
    private readonly IConfiguration _configuration;
    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmail(string to, string subject, string body)
    {
        string? email = _configuration.GetValue<string>("EmailConfig:Email");
        string? password = _configuration.GetValue<string>("EmailConfig:Password");
        string? host = _configuration.GetValue<string>("EmailConfig:Host");
        int port = _configuration.GetValue<int>("EmailConfig:Port");
        
        var smtpClint = new SmtpClient(host, port);
        smtpClint.EnableSsl = true;
        smtpClint.UseDefaultCredentials = false;
        smtpClint.Credentials = new NetworkCredential(email, password);
        var message = new MailMessage(email, to, subject, body);
        await smtpClint.SendMailAsync(message);
    }
}