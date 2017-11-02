using System.Web;
using System.Web.Mvc;
using log4net;

namespace EPWI.Web.Filters
{
  public class LogErrorAttribute : FilterAttribute, IExceptionFilter
  {
    #region IExceptionFilter Members
    private static readonly ILog log = LogManager.GetLogger("EPWI.Web");

    public void OnException(ExceptionContext filterContext)
    { 
      log.Error(
          $"An error has occurred in {filterContext.Controller.GetType().Name}. User Name: {HttpContext.Current.User.Identity.Name}", filterContext.Exception);
      System.Diagnostics.Debug.WriteLine(filterContext.Exception.Message);
    }

    #endregion
  }
}
