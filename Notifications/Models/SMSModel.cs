﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.Models
{
    public class SMSModel:NotificationBaseModel
    {
        public string Message { get; set; }
        public string SenderMobile { get; set; }
        public string RecieverMobile { get; set; }
    }
}