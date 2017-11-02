using System.Web.Optimization;

namespace EPWI.Web
{
    public class BundlesFormat
    {
        public const string Print = @"<link href=""{0}"" rel=""stylesheet"" type=""text/css"" media=""print"" />";
        public const string All = @"<link href=""{0}"" rel=""stylesheet"" type=""text/css"" media=""all"" />";
        public const string Screen = @"<link href=""{0}"" rel=""stylesheet"" type=""text/css"" media=""screen"" />";
    }

    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = false;

            bundles.Add(new StyleBundle("~/Content/bootstrap")
                .Include("~/Content/bootstrap.css"));


            bundles.Add(new StyleBundle("~/Content/css/epwi").Include(
                "~/Content/css/AccountStatus.css",
                "~/Content/css/colorbox.css",
                "~/Content/css/DynamicData.css",
                "~/Content/css/epwi.css",
                "~/Content/css/kits.css",
                "~/Content/css/stockstatus.css",
                "~/Content/css/superfish.css",
                "~/Content/css/YUIPaginatorDatatable.css"));

            bundles.Add(new StyleBundle("~/Content/css/print").Include(
                "~/Content/css/print.css"));

            bundles.Add(new StyleBundle("~/Content/css/screen").Include(
                "~/Content/css/superfish.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/jquery-ui").Include(
                "~/Content/themes/base/*.css"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-ui").Include(
                "~/Scripts/jquery.ui-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/epwi")
                .Include("~/Scripts/underscore.js")
                .Include("~/Scripts/knockout-3.4.0.js")
                .Include("~/Scripts/site/amplify.core.min.js")
                .Include("~/Scripts/site/amplify.store.min.js")
                .Include("~/Scripts/jquery.unobtrusive-ajax.min.js")
                .Include("~/Scripts/jquery-ui-1.12.0-rc.2.js")
                .Include("~/Scripts/jquery.cycle.lite.1.0.min.js")
                .Include("~/Scripts/jquery-migrate-1.2.1.min.js")
                .Include("~/Scripts/superfish_supersubs.js")
                .IncludeDirectory(
                    "~/Scripts/site/", "*.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js"));
        }
    }
}