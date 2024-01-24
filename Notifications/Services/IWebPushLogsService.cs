using Notifications.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.Services
{
    public interface IWebPushLogsService
    {
        Task<WebPushLogs> InsertAsync(WebPushLogs item);
        public int GetUserNotificationsCount(string userName);
        public List<WebPushLogs>? GetUserNotifications(string  userName);
        public Task<bool> MarkNotificationAsSeen(int notificationId);
        List<WebPushLogs>? GetTopNotification(string userName , int count);
    }
}
