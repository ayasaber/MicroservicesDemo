using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Notifications.Entities
{
    /// <summary>
    /// Database Tabel: <see cref="TableNames.NotificationType"/>
    /// </summary>
    public class NotificationType
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string ArName { get; set; }
        [MaxLength(50)]
        public string EnName { get; set; }
    
        public virtual ICollection<NotificationTemplate> NotificationTemplates { get; set; }

    }
}
