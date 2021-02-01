using System.Web;
using System.Web.Optimization;

namespace BwinoTips.WebUI
{
    public class BundleConfig
    {
      
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new StyleBundle("~/Content/styles/core").Include(
                  "~/Content/coreAssets/plugins/bootstrap/css/bootstrap.min.css",
                   "~/Content/coreAssets/plugins/metisMenu/metisMenu.min.css",
                   "~/Content/coreAssets/plugins/fontawesome/css/all.min.css",
                   "~/Content/coreAssets/plugins/typicons/src/typicons.min.css",
                   "~/Content/coreAssets/plugins/themify-icons/themify-icons.min.css",
                   "~/Content/coreAssets/plugins/datatables/dataTables.bootstrap4.min.css",
                   "~/Content/coreAssets/dist/css/style.css",
                   "~/Content/bootstrap-datetimepicker.css"));

            bundles.Add(new ScriptBundle("~/bundles/sitescripts").Include(
                        "~/Content/coreAssets/plugins/jQuery/jquery-3.4.1.min.js",
                        "~/Content/coreAssets/dist/js/popper.min.js",
                        "~/Content/coreAssets/plugins/bootstrap/js/bootstrap.min.js",
                        "~/Content/coreAssets/plugins/metisMenu/metisMenu.min.js",
                        "~/Content/coreAssets/plugins/perfect-scrollbar/dist/perfect-scrollbar.min.js",
                        "~/Content/coreAssets/plugins/chartJs/Chart.min.js",
                        "~/Content/coreAssets/plugins/sparkline/sparkline.min.js",
                        "~/Content/coreAssets/plugins/datatables/dataTables.min.js",
                        "~/Content/coreAssets/plugins/datatables/dataTables.bootstrap4.min.js",
                        "~/Content/coreAssets/dist/js/pages/dashboard.js",
                        "~/Content/coreAssets/dist/js/sidebar.js",
                        "~/Scripts/bootstrap-datepicker.js",
                        "~/Scripts/moment.js",
                        "~/Scripts/respond.js",
                        "~/Scripts/bootstrap-datetimepicker.js"));

                   
            bundles.Add(new StyleBundle("~/Content/styles/login").Include(
                     "~/Content/loginAssets/css/font-awesome.min.css",
                     "~/Content/loginAssets/css/style.css"));


        }
    }
}
