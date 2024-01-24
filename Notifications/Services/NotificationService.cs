
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Notifications.Context;
using Notifications.Services;
using Notifications.Entities;
using Notifications.Models;
using Notifications.Enum;
using Notifications.StaticKeys;

namespace Notifications.Services
{
    public class NotificationService 
    {
        private NotificationDbContext _context;
        private IEmailLogsService emailLogsService;
        private ISMSLogsService smsLogsService;
        private IWebPushLogsService webPushLogsService;
        private readonly IConfiguration configuration;
        private readonly IEmailService _emailService;
        private EmailSettings _emailSettings;

        public NotificationService(NotificationDbContext _eFContext, IEmailLogsService _emailLogsService, ISMSLogsService _smsLogsService, IWebPushLogsService _webPushLogsService, IConfiguration _configuration, IEmailService emailService)
        {
            this._context = _eFContext;
            emailLogsService = _emailLogsService;
            smsLogsService = _smsLogsService;
            webPushLogsService = _webPushLogsService;
            configuration = _configuration;
            _emailService = emailService;
        }
        public bool SendEmail(SendEmailModel model)
        {
            _emailSettings = configuration.GetSection(EmailSettings.SectionKey).Get<EmailSettings>();

            var template = GetNotificationTemplate((int)NotificationTypeEnum.Email);
            if (template is null)
            {
                return false;
            }

            var user = GetUser(model.UserId);
            if (user is null)
            {
                return false;
            }
            var userLanguage = !string.IsNullOrEmpty(user.PreferredLanguage) ? user.PreferredLanguage : Languages.Ar;
            var body = "";

            #region SendEmail
            EmailModel emailModel = new EmailModel()
            {
                Subject = model.Subject,
                IsBodyHtml = true,
                To = new List<string>() { model.To }
            };


            if (userLanguage == Languages.En)
            {
                emailModel.Subject = template.EnTitle;
                emailModel.Body = template.EnContent;
            }
            else
            {
                emailModel.Subject = template.ArTitle;
                emailModel.Body = template.ArContent;
            }
            if (template.EnTitle == TemplateTitles.ResetPasswordTemplate)
            {

                emailModel.Body = emailModel.Body.Replace("{{USERNAME}}", user.UserName);
            }

            body = emailModel.Body;
            _emailService.SendEmail(emailModel);

            #endregion

            #region InsertLog

            var emailLog = new EmailLogs()
            {
                To = model.To,
                BCC = model.BCC,
                CC = model.CC,
                Subject = model.Subject,
                Body = body,
                IsSent = true
            };

            emailLogsService.InsertAsync(emailLog);
            #endregion

            return true;
        }
        public bool SendSMS(SMSModel model)
        {
            #region SendSMS

            #endregion
            #region InsertLog

            var smsLog = new SMSLogs()
            {
                Message = model.Message,
                RecieverMobile = model.RecieverMobile,
                SenderMobile = model.SenderMobile,
                Subject = model.Subject,
                IsSent = true
            };

            smsLogsService.InsertAsync(smsLog);

            #endregion

            return true;
        }
        public bool SendWebPush(WebPushModel model)
        {
            var template = GetNotificationTemplate((int)NotificationTypeEnum.WebPush);
            if (template is null)
            {
                return false;
            }

            var user = GetUser(model.UserId);
            if (user is null)
            {
                return false;
            }
            var userLanguage = !string.IsNullOrEmpty(user.PreferredLanguage) ? user.PreferredLanguage : Languages.Ar;

            #region ConstructNotificationObject

            #endregion

            #region InsertLog

            var webPushLog = new WebPushLogs()
            {
                Link = model.Link,
                UserId = model.UserId,

            };
            if (userLanguage == Languages.En)
            {
                webPushLog.TitleEn = template.EnTitle ;
                webPushLog.DescriptionEn = template.EnContent;
            }
            else
            {
                webPushLog.TitleAr = template.ArTitle;
                webPushLog.DescriptionAr = template.ArContent;
            }
            webPushLogsService.InsertAsync(webPushLog);
            #endregion

            return true;
        }
        private NotificationTemplate? GetNotificationTemplate(int notificationTemplateCode)
        {
            return _context.NotificationTemplates.FirstOrDefault(q => q.NotificationCode == notificationTemplateCode);
        }
        private User? GetUser(int userId)
        {
            return _context.Users.FirstOrDefault(u => u.Id == userId);
        }
    }
}
