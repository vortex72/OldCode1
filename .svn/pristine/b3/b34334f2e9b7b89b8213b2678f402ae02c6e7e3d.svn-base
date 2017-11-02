using System;
using System.Linq;
using System.Web.Mvc;
using System.Threading;
using EPWI.Components.Models;
using log4net;
using System.Xml;
using System.Xml.Linq;

namespace EPWI.Web.Filters
{
  public class LogRequestAttribute : ActionFilterAttribute, IActionFilter
  {
    private static readonly ILog log = LogManager.GetLogger("LogRequestAttribute");
    #region IActionFilter Members

    void IActionFilter.OnActionExecuted(ActionExecutedContext filterContext)
    {

    }

    void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
    {
      var rep = new LogRepository();

      ThreadPool.QueueUserWorkItem(delegate
      {
        try
        {
          XDocument doc = null;

          if (filterContext.ActionParameters != null && filterContext.ActionParameters.Any())
          {
            doc = new XDocument();

            using (var writer = doc.CreateWriter())
            {
              writer.WriteStartDocument();
              writer.WriteStartElement("parameters");

              foreach (var parameter in filterContext.ActionParameters)
              {
                // don't log the password
                if (parameter.Key != "password")
                {
                  writer.WriteStartElement("parameter");
                  writer.WriteElementString("name", parameter.Key);
                  writer.WriteElementString("value", parameter.Value == null ? null : parameter.Value.ToString());
                  writer.WriteEndElement();
                }
              }

              writer.WriteEndElement();
              writer.WriteEndDocument();
              writer.Flush();
              writer.Close();
            }
          }
          var logEntry = new ActivityLog
          {
            Action = filterContext.ActionDescriptor.ActionName,
            Controller = filterContext.Controller.GetType().Name,
            //IPAddress = filterContext.HttpContext.Request.UserHostAddress,
            UserName = filterContext.HttpContext.User?.Identity.Name,
            ActionData = doc?.Root,
            ActionDate = DateTime.Now
          };
          rep.AddActionLogEntry(logEntry);
          rep.Save();
        }
        catch (System.Exception ex)
        {
          log.Warn("Error logging action", ex);
        }
      });
    }

    #endregion
  }
}
