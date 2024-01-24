using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.Models
{
    public class NotificationBaseModel
    {
        public int UserId { get; set; }
        public string Language { get; set; }
        public string Subject { get; set; }
        public int NotificationTemplateId { get; set; }
    }
}
