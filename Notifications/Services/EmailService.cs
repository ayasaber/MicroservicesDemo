using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Notifications.Context;
using Notifications.Models;

namespace Notifications.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        IConfiguration _configuration;
        private NotificationDbContext eFContext;
        private readonly ILogger<EmailService> _logger;
        public EmailService( IConfiguration configuration,  NotificationDbContext _eFContext, ILogger<EmailService> logger) 
        {
            //_emailSettings = emailSettings;
            _configuration = configuration;
            eFContext = _eFContext;
            _logger = logger;
        }
        public bool PrepareResetPasswordEmail(List<string> emails, int id, string token, string FirstName)
        {
            EmailModel emailModel = new EmailModel();
            var subject = "LMS Reset Password";
            var actionMethod = "resetPassword?id={0}&token={1}&email={2}";
            var href = string.Format(this._configuration.GetSection("EmailSettings").GetValue<string>("http") + actionMethod, id,token, emails[0]).ToString();

            var link = "<a href='" + href + "'>Reset Password</a>";
            var body = string.Format("<p>Dear {0} ,<br/>We have received a password change request for your account .</p>" +
                "<p>If you did not ask to change your password, then you can ignore this email and your password will not be changed.</p>"
                ,FirstName) +link+
                               "<p>Regards</p>";
            emailModel.Subject = subject;
            emailModel.Body = body;
            emailModel.To = emails;
            return this.SendEmail(emailModel);
        }
        public bool SendEmail(EmailModel emailModel)
        {
            try
            {
                SmtpClient smtp = new SmtpClient()
                {
                    Host = _emailSettings.Host,
                    Port = _emailSettings.Port,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = _emailSettings.UseDefaultCredentials,
                    Credentials = new NetworkCredential(_emailSettings.From, _emailSettings.Password),
                    EnableSsl = _emailSettings.EnableSsl
                };
                MailMessage mailMessage = new MailMessage()
                {
                    From = new MailAddress(_emailSettings.From),
                    Body = emailModel.Body,
                    Subject = emailModel.Subject,
                    IsBodyHtml = true,
                };
                emailModel.To.ForEach(to =>
                {
                    mailMessage.To.Add(to);
                });
                if(emailModel.Cc.Count> 0)
                {
                    emailModel.Cc.ForEach(cc =>
                    {
                        mailMessage.CC.Add(cc);
                    });
                }
                smtp.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Response Error Message :  {ex.Message}");
                return false;
            }
        }
    }
}