using Notifications.Context;
using Notifications.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.Services
{
    public class EmailLogsService : IEmailLogsService
    {
        private NotificationDbContext eFContext;

        public EmailLogsService(NotificationDbContext _eFContext)
        {
            eFContext = _eFContext;
        }

        public async Task<EmailLogs> InsertAsync(EmailLogs item)
        {
            if (item == null) { return null; }
            await eFContext.EmailLogs.AddAsync(item);
            await eFContext.SaveChangesAsync();
            return item;
        }
    }
}
