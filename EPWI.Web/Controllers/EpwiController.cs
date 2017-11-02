using System;
using System.Web;
using System.Web.Mvc;
using EPWI.Components.Exceptions;
using EPWI.Components.Models;
using EPWI.Web.Exception;
using EPWI.Web.Filters;

namespace EPWI.Web.Controllers
{
    [LogError(Order = 1)]
    [RedirectOnError(Order = 2)]
    [HandleError(ExceptionType = typeof (AccessDeniedException), View = "AccessDenied", Order = 3)]
    [HandleError(ExceptionType = typeof (KitNoNipcCodeFoundException), View = "InvalidKit", Order = 4)]
    [HandleError(Order = 5)]
    public class EpwiController : Controller
    {
        private ICustomerData customerData;

        protected ICustomerData CustomerData
        {
            get
            {
                if (customerData == null)
                {
                    var rep = new UserRepository();
                    customerData = rep.GetByUserName(HttpContext.User.Identity.Name);

                    if (User.Identity.IsAuthenticated && customerData == null)
                    {
                        throw new UserInvalidException();
                    }
                }
                return customerData;
            }
        }

        public bool CurrentUserIsEmployee
        {
            get { return User.IsInRole("EMPLOYEE"); }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }

        protected int GetViewCookie()
        {
            int currentView = 1;

            if (Request.Cookies["currentView"] != null)
            {
                int.TryParse(Request.Cookies["currentView"].Value, out currentView);
            }
            else
            {
                WriteViewCookie(currentView);
            }

            return currentView;
        }


        protected void WriteViewCookie(int view)
        {
            Response.Cookies.Add(new HttpCookie("currentView", view.ToString()) {Expires = DateTime.Now.AddDays(14)});
        }

        protected JavaScriptResult ClientRedirectResult(string url)
        {
            return JavaScript($"displayProcessingBlock(); window.location = '{url}';");
        }
    }
}