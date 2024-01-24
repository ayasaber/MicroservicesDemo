using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.Entities
{
    /// <summary>
    /// Database Tabel: <see cref="TableName.SMSLogs"/>
    /// </summary>

    public class SMSLogs
    {
        public int Id { get; set; }
        [MaxLength(20)]
        public string SenderMobile { get; set; }
        [MaxLength(20)]
        public string RecieverMobile { get; set; }
        [MaxLength(50)]
        public string Subject { get; set; }
        public string Message { get; set; }
        public bool IsSent { get; set; }
    }
}
