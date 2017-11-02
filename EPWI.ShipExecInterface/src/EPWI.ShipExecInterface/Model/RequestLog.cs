using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPWI.ShipExecInterface
{
    /// <summary>
    /// Model for logging request results
    /// </summary>
    public class RequestLog
    {
        public string RequestType { get; set; }
        public int RequestID { get; set; }
        public string RequestBody { get; set; }
        public string ResponseBody { get; set; }
        public string ErrorMessage { get; set; }

    }

    public class RequestType
    {
        public const string RateRequest = "rate";
        public const string ShipRequest = "ship";

    }
}
