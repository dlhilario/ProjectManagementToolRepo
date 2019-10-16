using System.Web;
using System.Web.Optimization;

namespace ProjectManagementTool
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.mobile-1.5.4-alpah.1.min.js",
                        "~/Scripts/jquery.unobtrusive-ajax.min.js",
                        "~/Scripts/jquery.validate.min.js",
                        //"~/Scripts/jquery.validate.unobtrusive.min.js",
                        "~/Scripts/additional-methods.min.js",
                        "~/Scripts/moment.min.js",
                        "~/js/app.js"
                        ));

            //bundles.Add(new ScriptBundle("~/bundles/angular").Include(
            //    "~/Scripts/angular.min.js" , 
            //    "~/Angular/apps.js",   
            //    "~/Scripts/ui-grid.js",
            //    "~/Scripts/angular-route.js",
            //    "~/Scripts/angular-touch.js",
            //    "~/Scripts/angular-animate.js",
            //    "~/Scripts/angular-cookies.min.js",
            //    "~/Angular/controller/MainController.js",
            //    "~/Scripts/angular-loader.min.js",
            //    "~/Scripts/angular-message-format.min.js",
            //    "~/Scripts/angular-messages.min.js",
            //    "~/Scripts/angular-resource.min.js"
            //));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/popper-min.js",
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/bootstrap-datepicker.min.js"
                      ));


            bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
                      "~/Scripts/DataTables/jquery.dataTables.min.js",
                      "~/Scripts/DataTables/buttons.print.min.js",
                      "~/Scripts/DataTables/dataTables.scroller.min.js",
                      "~/Scripts/DataTables/dataTables.responsive.min.js",
                      "~/Scripts/DataTables/dataTables.rowReorder.min.js"
                     ));

            bundles.Add(new ScriptBundle("~/js/layout").Include(
                      "~/js/jquery.layout-latest.js"
                     ));
            bundles.Add(new ScriptBundle("~/js/piechart").Include(
                      "~/js/jquery.throttledresize.js",
                      "~/js/pieChart.js"
                     ));


            bundles.Add(new StyleBundle("~/content/layout").Include(
                    "~/Content/layout-default-latest.css"
                ));



            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/Themes/base/all.css",
                      "~/Content/jquery.mobile-1.4.5.min.css",
                      "~/Content/jquery.mobile.theme-1.4.5.min.css",
                      "~/Content/DataTables/css/jquery.dataTables.min.css",
                      "~/Content/font-awesome.css",
                      "~/Content/bootstrap.min.css",
                      "~/Content/bootstrap-reboot.css",
                      "~/Content/bootstrap-datepicker.min.css",
                      "~/Content/site.css"));
        }
    }
}
