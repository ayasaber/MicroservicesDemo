using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging.Models
{
    [Serializable]
    public class ApiResponse : ApiResponseMini
    {
        public ApiResponse()
        {
            Status = 200;
        }

        public ApiResponse(bool isError, string message, string errorMessage, Error error, object data, string method, int status = 200, string? eventID = null)
        {
            IsError = isError;
            Error = error;
            Message = message;
            ErrorMessage = errorMessage;
            Data = data;
            Status = status;
            Method = method;
            EventID = eventID == null && isError ? Guid.NewGuid().ToString() : null;
        }
        public ApiResponse(bool isError, string message, string errorMessage, List<string> errorMessages, Error error, object data, string method, int status = 200, string? eventID = null)
        {
            IsError = isError;
            Error = error;
            Message = message;
            ErrorMessage = errorMessage;
            Data = data;
            Status = status;
            Method = method;
            ErrorMessages = errorMessages;
            EventID = eventID == null && isError ? Guid.NewGuid().ToString() : null;
        }

        public ApiResponse(bool isError, string message, string errorMessage, Error error, int status = 200, string? eventID = null)
        {
            IsError = isError;
            Error = error;
            Message = message;
            ErrorMessage = errorMessage;
            Status = status;
            EventID = eventID == null && isError ? Guid.NewGuid().ToString() : null;
        }

        public bool IsError { get; set; } = false;
        public Error Error { get; set; } = null;
        public string? EventID { get; set; } = null;
        public List<string> ErrorMessages { get; set; }
    }
    public class ApiResponseMini
    {
        public int? Status { set; get; }
        public string Method { get; set; }

        public string ErrorMessage { get; set; }
        public string Message { set; get; }
        public object Data { set; get; }
    }

    public class Error
    {
        [DefaultValue(null)]
        public ErrorLocation Location { get; set; }
        public List<ErrorControl> Controls { set; get; }
    }

    public class ErrorControl
    {
        public string PropertyName { get; set; }
        public string Error { get; set; }
    }

    public enum ErrorLocation
    {
        application, page, form, controls
    }
}
