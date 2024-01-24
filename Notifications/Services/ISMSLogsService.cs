using Notifications.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.Services
{
    public interface ISMSLogsService
    {
        Task<SMSLogs> InsertAsync(SMSLogs item);
    }
}
