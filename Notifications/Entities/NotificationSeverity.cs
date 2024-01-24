using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;


namespace Notifications.Entities
{
    /// <summary>
    /// Database Tabel: <see cref="TableNames.NotificationSeverity"/>
    /// </summary>
    public class NotificationSeverity//:BaseEntity_HardDelete
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string Code { get; set; }
    
        public virtual ICollection<NotificationTemplate>NotificationTemplates { get; set; }
  
        public virtual ICollection<NotificationType> NotificationTypes { get; set; }
   
        public virtual ICollection<WebPushLogs> WebPushLogs { get; set; }
    }
}
