using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Logging.Models
{
    [Serializable]
    public class ModelErrorException : Exception
    {
        public string PropertyName { get; set; }

        public ModelErrorException(string propertyName)
        {
            PropertyName = propertyName;
        }

        public ModelErrorException(string propertyName, string message) : base(message)
        {
            PropertyName = propertyName;
        }

        public ModelErrorException(string propertyName, string message, Exception innerException) : base(message, innerException)
        {
            PropertyName = propertyName;
        }

        public ModelErrorException(string propertyName, SerializationInfo info, StreamingContext ctxt) : base(info, ctxt)
        {
            PropertyName = propertyName;
        }

    }
}
