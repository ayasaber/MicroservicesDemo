using Notifications.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.Services
{
    public class NotificationTemplateService:INotificationTemplateService
    {
        private NotificationDbContext _context;

        public NotificationTemplateService( NotificationDbContext _eFContext)
        {
            _context = _eFContext;
        }
    }
}
