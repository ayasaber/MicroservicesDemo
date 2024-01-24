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
    /// Database Tabel: <see cref="TableNames.NotificationTemplate"/>
    /// </summary>
    //[Table(TableNames.NotificationTemplate)]
    public class NotificationTemplate//:BaseEntity_HardDelete
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string ArTitle { get; set; }
        [MaxLength(50)]
        public string? EnTitle { get; set; }
        public string? ArContent { get; set; }
        public string? EnContent { get; set; }
        public int NotificationTypeId { get; set; }
        public int NotificationCode { get; set; }
        public int NotificationSeverityId { get; set; }
        [ForeignKey("NotificationTypeId")]


        public NotificationType? NotificationType { get; set; }

        [ForeignKey("NotificationSeverityId")]

        public NotificationSeverity? NotificationSeverity { get; set; }

    }
}
