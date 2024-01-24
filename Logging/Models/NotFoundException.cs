﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Logging.Models
{
    [Serializable]
    public class NotFoundException : Exception
    {
        public NotFoundException() : base("Not Found!") { }
        public NotFoundException(string message) : base(message) { }
        public NotFoundException(string message, Exception innerException) : base(message, innerException) { }

        public NotFoundException(SerializationInfo info, StreamingContext ctxt) : base(info, ctxt) { }

    }
}
