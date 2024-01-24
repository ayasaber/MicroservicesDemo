using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging
{
    public static class LogContextProperties
    {
        #region Log Properties Const
        public static readonly string REQUEST_BODY = "Request.Body";
        public static readonly string REQUEST_HEADER = "Request.Headers";
        public static readonly string ACTION_ARGS = "Action Args";
        public static readonly string HTTPMETHOD = "HttpMethod";
        public static readonly string REQUEST_QUERYSTRING = "Request.QueryString";
        public static readonly string SCHEMA = "Schema";
        public static readonly string HOST = "Host.Number";
        public static readonly string HOST_REMOTE = "Host.Remote";
        //public static readonly string HOST = "Host";

        public static readonly string USERNAME = "Username";
        public static readonly string IPADDRESS_LOCAL = "IPAddress.Local";
        public static readonly string IPADDRESS_REMOTE = "IPAddress.Remote.Number";
        public static readonly string IPADDRESS_REMOTE_GATEWAY = "IPAddress.Remote.Gateway";
        public static readonly string PAGE = "Page";
        public static readonly string SYSTEM = "System";
        public static readonly string MODULE = "Module";
        public static readonly string LOGTYPE = "LogType";
        public static readonly string MODELERRORS = "ModelErrors";
        public static readonly string ELAPSED = "Elapsed";
        public static readonly string MESSAGE = "Message";
        public static readonly string EVENTID = "EventID";
        public static readonly string RESPONSE_BODY = "Response.Body";
        public static readonly string STATUSCODE = "StatusCode";
        #endregion Log Properties Const
    }
}
