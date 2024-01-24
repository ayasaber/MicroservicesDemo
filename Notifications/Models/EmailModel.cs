using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.Models
{
    public class EmailModel: NotificationBaseModel
    {
        public List<string> To { set; get; }
        public List<string> Cc { set; get; } = new List<string>();
        public List<string> Bcc { set; get; } = new List<string>();
        public string Subject { set; get; }
        public bool IsBodyHtml { set; get; }
        public string Body { set; get; }
        public bool IsOTP { set; get; }

    }
    public class SendEmailModel : NotificationBaseModel
    {
        public string To { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
        public string Body { get; set; }

    }
}
