using System.Web.Mvc;
using EPWI.Web.Exception;
using System.Web.Security;
using log4net;

namespace EPWI.Web.Filters
{
  public class RedirectOnErrorAttribute : FilterAttribute, IExceptionFilter
  {
    #region IExceptionFilter Members
    private static readonly ILog log = LogManager.GetLogger("RedirectOnErrorAttribute");

    public void OnException(ExceptionContext filterContext)
    {
      // Don't interfere if exception is already handled
      if (filterContext.ExceptionHandled)
        return;

      if (filterContext.Exception is UserInvalidException)
      {
        log.Warn("User is invalid. Redirecting to login page");
        filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { controller = "Account", action = "LogOn" }));

        // advise subsequent exception filters not to interfere
        // and stop ASP.NET from producing a "yellow screen of death"
        filterContext.ExceptionHandled = true;
        FormsAuthentication.SignOut();

        // erase any output already generated
        filterContext.HttpContext.Response.Clear();
      }
    }

    #endregion
  }
}