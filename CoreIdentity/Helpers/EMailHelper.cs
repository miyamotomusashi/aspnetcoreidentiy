using CoreIdentity.Models.Email;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Runtime;

namespace CoreIdentity.Helpers
{
  public class EMailHelper
  {
    private readonly EmailSettings _settings;

    public EMailHelper(IOptions<EmailSettings> options)
    {
      _settings = options.Value;
    }

    public async Task SendEmail(EMailModel model)
    {
      var mail = new MailMessage
      {
        Subject = model.Subject,
        Body = model.Body,
        From = new MailAddress(_settings.UserName),
        IsBodyHtml = true
      };
      mail.To.Add(model.To);

      var smtp = new SmtpClient
      {
        Host = "smtp.gmail.com",
        Port = 587,
        EnableSsl = true,
        DeliveryMethod = SmtpDeliveryMethod.Network,
        UseDefaultCredentials = false,
        Credentials = new NetworkCredential(_settings.UserName, _settings.Password)
      };

    

      await smtp.SendMailAsync(mail);
    }
  }
}
