using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Logging.Models
{
    [Serializable]
    public class ResponseException : Exception
    {
        public ApiResponse Response { get; }

        public ResponseException(ApiResponse response) : base()
        {
            Response = response;
        }
        public ResponseException(ApiResponse response, string message) : base(message)
        {
            Response = response;
        }
        public ResponseException(ApiResponse response, string message, Exception innerException) : base(message, innerException)
        {
            Response = response;
        }

        public ResponseException(ApiResponse response, SerializationInfo info, StreamingContext ctxt) : base(info, ctxt)
        {
            Response = response;
        }

    }
}
