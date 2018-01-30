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
                        "~/Scripts/jquery-ui-{version}.js",
                        "~/Scripts/DataTables/jquery.dataTables.min.js",
                        "~/Scripts/jquery.validate.min.js",
                        "~/Scripts/moment.js"
                        ));


            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/foundation").Include(
                      "~/dist/js/foundation.min.js"
                     ));
            bundles.Add(new ScriptBundle("~/js/layout").Include(
                      "~/js/jquery.layout-latest.js"
                     ));
            bundles.Add(new ScriptBundle("~/js/piechart").Include(
                      "~/js/jquery.throttledresize.js",
                      "~/js/pieChart.js"
                     ));
            bundles.Add(new StyleBundle("~/fonts/foundation/icons").Include(
                      "~/fonts/foundation-icons/foundation-icons.css"
                      ));

            bundles.Add(new StyleBundle("~/content/layout").Include(
                    "~/Content/layout-default-latest.css"
                ));

            bundles.Add(new StyleBundle("~/scss/foundation").Include(
                    "~/dist/css/foundation.min.css"
                ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/themes/base/jquery.ui.css",
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-grid.css",
                      "~/Content/bootstrap-reboot.css",
                      "~/Content/font-awesome.css",

                      "~/Content/DataTables/css/dataTables.foundation.min.css",
                      "~/Content/DataTables/css/jquery.dataTables.min.css",
                      "~/Content/jquery.ui.layout.css",
                      "~/Content/site.css"));
        }
    }
}
