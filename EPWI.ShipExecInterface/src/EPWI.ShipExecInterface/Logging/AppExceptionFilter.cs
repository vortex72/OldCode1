using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace EPWI.ShipExecInterface
{

    /// <summary>
    /// Global Error handler
    /// </summary>
    //============================================================
    //Revision History
    //Date        Author          Description
    //02/27/2017  TB               Created
    //============================================================
    public class AppExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;
        private ISQLLogger _sqlLogger;

        /// <summary>
        /// DI Constructor
        /// </summary>
        /// <param name="loggerfactory"></param>
        /// <param name="SQLLogger"></param>
        //============================================================
        //Revision History
        //Date        Author          Description
        //02/27/2017  TB               Created
        //============================================================
        public AppExceptionFilterAttribute(ILoggerFactory loggerfactory, ISQLLogger SQLLogger)
        {
            _logger = loggerfactory.CreateLogger<AppExceptionFilterAttribute>();
            _sqlLogger = SQLLogger;

        }

        /// <summary>
        /// Exception Handler
        /// </summary>
        /// <param name="context"></param>
        //============================================================
        //Revision History
        //Date        Author          Description
        //02/27/2017  TB               Created
        //============================================================
        public override void OnException(ExceptionContext context)
        {
            //extract information from request
            HttpRequest request = context.HttpContext.Request;

            int requestId = -1;

            if (request.Query.ContainsKey("requestId"))
                int.TryParse(request.Query["requestId"], out requestId);

            string requestType = request.Path.HasValue ? (request.Path.Value.Contains("get_rates") ? RequestType.RateRequest : request.Path.Value.Contains("create_shipment") ? RequestType.ShipRequest : string.Empty) : string.Empty;
            string requestBody = (request.Path.HasValue ? request.Path.Value : string.Empty) + "/" + (request.QueryString.HasValue ? request.QueryString.Value : string.Empty);

            //log error
            RequestLog log = new RequestLog()
            {
                ErrorMessage = context.Exception.ToString(),
                RequestID = requestId,
                RequestType = requestType,
                RequestBody = requestBody
            };

            _sqlLogger.LogRequest(log);
        }
    }
}
