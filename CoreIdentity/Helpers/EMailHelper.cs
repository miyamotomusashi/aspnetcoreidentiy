using CoreIdentity.Models.Email;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Runtime;

namespace CoreIdentity.Helpers
{
  public class EMailHelper
  {
    private readonly EmailSettings settings;
    public EMailHelper(IOptions<EmailSettings> options)
    {
      this.settings = options.Value;
    }

    public async Task SendEmail(EMailModel mailModel)
    {
      var mail = new MailMessage()
      {
        Body = mailModel.Body,
        Subject = mailModel.Subject,
        From = new MailAddress(settings.UserName),
        IsBodyHtml = true
      };

      mail.To.Add(mailModel.To);

      var smtp = new SmtpClient
      {
        Host = "smtp.gmail.com",
        Port = 587,
        EnableSsl = true,
        DeliveryMethod = SmtpDeliveryMethod.Network,
        UseDefaultCredentials = false,
        Credentials = new NetworkCredential(settings.UserName, settings.Password)
      };

      await smtp.SendMailAsync(mail);

    }
  }
}
