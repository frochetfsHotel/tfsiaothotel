using System.Web;
using System.Web.Optimization;

namespace SuccessHotelierHub
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/css/bootstrap.min.css",
                      "~/Content/css/font-awesome/css/font-awesome.min.css",
                      "~/Content/css/Ionicons/css/ionicons.min.css",
                      "~/Content/css/AdminLTE/AdminLTE.css",
                      "~/Content/css/AdminLTE/skins/_all-skins.min.css",
                      "~/Content/css/Site.css",
                      "~/Content/css/responsive.css",
                      "~/Content/css/AdminLTE/bootstrap-datepicker.min.css",
                      "~/Content/css/AdminLTE/bootstrap-timepicker.min.css",
                      "~/Content/css/AdminLTE/toastr.min.css",
                      "~/Content/css/AdminLTE/sweetalert2/sweetalert2.css"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                      "~/Scripts/AdminLTE/bootstrap-datepicker.min.js",
                      "~/Scripts/AdminLTE/bootstrap-timepicker.min.js",
                      "~/Scripts/AdminLTE/adminlte.min.js",
                      "~/Scripts/AdminLTE/jquery-ui/jquery-ui.js",
                      "~/Scripts/AdminLTE/jquery-slimscroll/jquery.slimscroll.min.js",
                      "~/Scripts/AdminLTE/toastr.min.js",
                      "~/Scripts/AdminLTE/sweetalert2/sweetalert2.min.js",
                      "~/Scripts/AdminLTE/demo.js",                      
                      "~/Scripts/Custom/jquery.validate.date.js",                      
                      "~/Scripts/Custom/common.js"
                      ));

            BundleTable.EnableOptimizations = false;
        }
    }
}
