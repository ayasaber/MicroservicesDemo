using Notifications.Models;
using System;

namespace Notifications.Services
{
    public interface IEmailService 
    {
        bool SendEmail(EmailModel emailModel);
        bool PrepareResetPasswordEmail(List<string> emails, int id, string token, string FirstName);
    }
}
