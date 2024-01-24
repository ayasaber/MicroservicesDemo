using Microsoft.EntityFrameworkCore;
using Notifications.Context;
using Notifications.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.Services
{
    public class SMSLogsService:ISMSLogsService
    {
        private NotificationDbContext context;

        public SMSLogsService( NotificationDbContext _eFContext) 
        {
            context = _eFContext;
        }
        public async Task<SMSLogs> InsertAsync(SMSLogs item)
        {
            try
            {
                if (item == null) { return null; }
                await context.SMSLogs.AddAsync(item);
                await context.SaveChangesAsync();
                return item;
            }
            catch(Exception e)
            {
                return null;
            }
       
        }
    }
}
