using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web.DynamicData;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using EPWI.Components.Models;
using EPWI.Web.ModelBinders;
using EPWI.Web.Models;
using log4net;
using log4net.Config;
using N2.Definitions.Runtime;
using N2.Engine;
using N2.Security;
using N2.Web.Mvc;

namespace EPWI.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes, IEngine engine)
        {
            engine.RegisterAllControllers();
            ControllerBuilder.Current.SetControllerFactory(engine.GetControllerFactory());
            //engine.RegisterViewTemplates<ContentController>()
            //    .Add<ContentController>();

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{resource}.ashx/{*pathInfo}");
            //to allow a virtual that points to the legacy site
            routes.IgnoreRoute("legacy");
            //to allow a redirect to the production site
            routes.IgnoreRoute("internal");

            // dynamic data
            var model = new MetaModel();

            var cnnxn = ConfigurationManager.ConnectionStrings["EPWIConnectionString"];
            var foo = new EPWIDataContext(cnnxn.ConnectionString);
            model.RegisterContext(
                () => foo);

            // The following statement supports separate-page mode, where the List, Detail, Insert, and 
            // Update tasks are performed by using separate pages. To enable this mode, uncomment the following 
            // route definition, and comment out the route definitions in the combined-page mode section that follows.
            routes.Add(new DynamicDataRoute("DynamicData/{table}/{action}.aspx")
            {
                Constraints = new RouteValueDictionary(new { action = "List|Details|Edit|Insert" }),
                Model = model,
                RouteHandler = new CustomDynamicDataRouteHandler()
            });
            // end dynamic data
            
            //comment this route to show the Home page from N2CMS instead of Home/Index.aspx
            routes.MapRoute("Root", string.Empty, new { controller = "Home", action = "Index", id = "" });
            routes.MapRoute("Status", "AS400Check", new { controller = "Home", action = "AS400Check", id = "" });
            // This route detects content item paths and executes their controller
            routes.Add(new ContentRoute(engine));
            routes.MapRoute(null, "Kit/Build/{id}/{SelectedYear}/{SelectedKitType}",
                            new { controller = "Kit", action = "Build" });

            routes.MapRoute(null, "Account/Statement/{CustomerID}/{CompanyCode}/{Year}/{Month}", new { controller = "Account", action = "Statement", printable = false });

            routes.MapRoute(null, "Account/Invoice/{CustomerID}/{CompanyCode}/{InvoiceNumber}", new { controller = "Account", action = "Invoice" });

            routes.MapRoute(null, "MillionthPart/{CustomerID}/{CompanyCode}/{OrderNumber}/{ValidationCode}/{UserID}", new { controller = "MillionthPart", action = "Index" });

            routes.MapRoute(
                    null,
                    "Order/AddItem/{orderMethod1}/{warehouse1}/{quantity1}/{orderMethod2}/{warehouse2}/{quantity2}",
                    new { controller = "Order", action = "AddItem", warehouse1 = "XXX", orderMethod2 = (OrderMethod?)null, warehouse2 = (string)null, quantity2 = (int?)null });

            routes.MapRoute(
                    null,
                    "StockStatus/Search/{requestedItemNumber}/{requestedQuantity}/{requestedLineCode}",
                    new { controller = "StockStatus", action = "Search", requestedQuantity = (int?)null, requestedItemNumber = (string)null, requestedLineCode = (string)null });

            routes.MapRoute(
                    "Default",                                              // Route name
                    "{controller}/{action}/{id}",                           // URL with parameters
                    new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
            );
            //TODO: Add parameter validation as needed
        }

        protected void Application_Start()
        {
            XmlConfigurator.ConfigureAndWatch(new FileInfo(Server.MapPath("~/EPWI.Web.log4net")));
            ILog log = LogManager.GetLogger("EPWI.Web");
            log.Debug("Log4Net logging initialized");

            IEngine engine = N2.Context.Initialize(true);
            RegisterRoutes(RouteTable.Routes, engine);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            System.Web.Mvc.ModelBinders.Binders.Add(typeof(StockStatusRequest), new SessionModelBinder<StockStatusRequest>("_StockStatusRequest"));
            System.Web.Mvc.ModelBinders.Binders.Add(typeof(List<Warranty>), new SessionModelBinder<List<Warranty>>("_KitWarranty"));
            System.Web.Mvc.ModelBinders.Binders.Add(typeof(SessionStore), new SessionModelBinder<SessionStore>("_SessionStore"));
            //
        }

        public override void Init()
        {
            base.Init();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var error = Server.GetLastError();
            var permissionDenied = error as PermissionDeniedException;
            var app = sender as MvcApplication;
            if (permissionDenied != null && app != null)
            {
                Response.Redirect("~/Account/LogOn?ReturnUrl=" + app.Request.Url);
                return;
            }
            if (error != null)
            {
                var exception = error.GetBaseException();
            }
        }
        //protected void Application_AuthenticateRequest(object sender, EventArgs e)
        //{
        //  IPrincipal principal = HttpContext.Current.User;
        //  if (principal != null && principal.Identity.IsAuthenticated && !UserStillValid(principal))
        //  {
        //    IPrincipal anonymousPrincipal = new GenericPrincipal(new GenericIdentity(String.Empty), null);
        //    Thread.CurrentPrincipal = anonymousPrincipal;
        //    HttpContext.Current.User = anonymousPrincipal;
        //    FormsAuthentication.SignOut();
        //  }
        //}

        //private bool UserStillValid(IPrincipal principal)
        //{
        //  var userRep = new UserRepository();

        //  return (userRep.GetByUserName(principal.Identity.Name) != null);
        //}
    }
}