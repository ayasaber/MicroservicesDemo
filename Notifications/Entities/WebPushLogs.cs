using Notifications;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace Notifications.Entities
{
    /// <summary>
    /// Database Tabel: <see cref="TableName.WebPushLogs"/>
    /// </summary>
    public class WebPushLogs
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        [MaxLength(50)]
        public string TitleAr { get; set; }
        [MaxLength(50)]
        public string TitleEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        [MaxLength(150)]
        public string Link { get; set; }
        [DefaultValue(false)]
        public bool IsSeen { get; set; }
        public int NotificationSeverityId { get; set; }
        public DateTime Date { get; set; }
        public virtual User? User { get; set; }
        public virtual NotificationSeverity? NotificationSeverity { get; set; }
    }

    }
