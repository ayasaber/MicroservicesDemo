using Notifications.Context;
using Notifications.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Notifications.Services
{
    public class WebPushLogsService : IWebPushLogsService
    {
        private NotificationDbContext eFContext;

        public WebPushLogsService(NotificationDbContext _eFContext)
        {
            eFContext = _eFContext;

        }

        public List<WebPushLogs>? GetUserNotifications(string userName)
        {
            var user = GetUser(userName);
            if (user == null) return null;

            return eFContext.WebPushLogs.Where(q => q.UserId == user.Id).ToList();
        }
        public List<WebPushLogs>? GetTopNotification(string userName, int count)
        {
            var user = GetUser(userName);
            if (user == null) return null;
            return eFContext.WebPushLogs.Where(q => q.UserId == user.Id).Take(count).ToList();
        }

        public int GetUserNotificationsCount(string userName)
        {
            var user = GetUser(userName);
            if (user == null) return 0;

            return eFContext.WebPushLogs.Where(q => q.UserId == user.Id && !q.IsSeen).Count();
        }

        public async Task<WebPushLogs> InsertAsync(WebPushLogs item)
        {

            if (item == null) { return null; }
            await eFContext.WebPushLogs.AddAsync(item);
            await eFContext.SaveChangesAsync();
            return item;
        }

        public async Task<bool> MarkNotificationAsSeen(int notificationId)
        {
            var notification = eFContext.WebPushLogs.FirstOrDefault(r => r.Id == notificationId);
            if (notification is null) return false;

            notification.IsSeen = true;
            eFContext.WebPushLogs.Update(notification);

            return true;
        }

        private User? GetUser(object userData)
        {

            if (userData is long)
                return eFContext.Users.FirstOrDefault(q => q.Id == (long)userData);

            return eFContext.Users.FirstOrDefault(w => w.UserName == (string)userData);

        }
    }
}
