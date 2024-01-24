using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.Entities
{
    public class EmailLogs
    {
        public int Id { get; set; }
        [MaxLength(250)]
        public string To { get; set; }
        [MaxLength(250)]
        public string? CC { get; set; }
        [MaxLength(250)]
        public string? BCC { get; set; }
        [MaxLength(50)]
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsSent { get; set; }
      
    }
}
