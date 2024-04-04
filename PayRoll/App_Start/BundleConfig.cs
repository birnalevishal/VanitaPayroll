using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.UI;

namespace PayRoll
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkID=303951
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/WebFormsJs").Include(
                            "~/Scripts/WebForms/WebForms.js",
                            "~/Scripts/WebForms/WebUIValidation.js",
                            "~/Scripts/WebForms/MenuStandards.js",
                            "~/Scripts/WebForms/Focus.js",
                            "~/Scripts/WebForms/GridView.js",
                            "~/Scripts/WebForms/DetailsView.js",
                            "~/Scripts/WebForms/TreeView.js",
                            "~/Scripts/WebForms/WebParts.js"));

            // Order is very important for these files to work, they have explicit dependencies
            bundles.Add(new ScriptBundle("~/bundles/MsAjaxJs").Include(
                    "~/Scripts/WebForms/MsAjax/MicrosoftAjax.js",
                    "~/Scripts/WebForms/MsAjax/MicrosoftAjaxApplicationServices.js",
                    "~/Scripts/WebForms/MsAjax/MicrosoftAjaxTimer.js",
                    "~/Scripts/WebForms/MsAjax/MicrosoftAjaxWebForms.js"));

            // Use the Development version of Modernizr to develop with and learn from. Then, when you’re
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                            "~/Scripts/modernizr-*"));

            ScriptManager.ScriptResourceMapping.AddDefinition(
                "respond",
                new ScriptResourceDefinition
                {
                    Path = "~/Scripts/respond.min.js",
                    DebugPath = "~/Scripts/respond.js",
                });


            bundles.Add(new StyleBundle("~/Content/vendor").Include(
                     "~/vendor/fontawesome/css/font-awesome.css",
                     "~/vendor/metisMenu/dist/metisMenu.css",
                     "~/vendor/animate.css/animate.css",
                     "~/vendor/bootstrap/dist/css/bootstrap.css",
                     "~/vendor/sweetalert/lib/sweet-alert.css",
                     "~/vendor/datatables.net-bs/css/dataTables.bootstrap.min.css",
                     "~/vendor/bootstrap-datepicker-master/dist/css/bootstrap-datepicker3.min.css"
                     ));

            bundles.Add(new StyleBundle("~/Content/app").Include(
                    "~/fonts/pe-icon-7-stroke/css/pe-icon-7-stroke.css",
                    "~/fonts/pe-icon-7-stroke/css/helper.css",
                    "~/styles/style.css",
                    "~/styles/static_custom.css"
                    ));

            bundles.Add(new ScriptBundle("~/bundles/vendor1").Include(
                            "~/vendor/jquery/dist/jquery.min.js"
                            ));

            bundles.Add(new ScriptBundle("~/bundles/vendor2").Include(
                           "~/vendor/jquery-ui/jquery-ui.min.js",
                           "~/vendor/slimScroll/jquery.slimscroll.min.js",
                           "~/vendor/bootstrap/dist/js/bootstrap.min.js"
                           ));

            bundles.Add(new ScriptBundle("~/bundles/vendor3").Include(
                           "~/vendor/jquery-flot/jquery.flot.js",
                           "~/vendor/jquery-flot/jquery.flot.resize.js",
                           "~/vendor/jquery-flot/jquery.flot.pie.js",
                           "~/vendor/flot.curvedlines/curvedLines.js",
                           "~/vendor/jquery.flot.spline/index.js"
                           ));

            bundles.Add(new ScriptBundle("~/bundles/vendor4").Include(
                           "~/vendor/metisMenu/dist/metisMenu.min.js",
                           "~/vendor/iCheck/icheck.min.js",
                           "~/vendor/peity/jquery.peity.min.js",
                           "~/vendor/sparkline/index.js"
                           ));

            bundles.Add(new ScriptBundle("~/bundles/vendor5").Include(
                          "~/vendor/jquery-validation/jquery.validate.min.js",
                          "~/vendor/sweetalert/lib/sweet-alert.min.js",
                          "~/vendor/toastr/build/toastr.min.js",
                          "~/vendor/bootstrap-datepicker-master/dist/js/bootstrap-datepicker.min.js"
                          ));

            bundles.Add(new ScriptBundle("~/bundles/vendor6").Include(
                          "~/vendor/datatables/media/js/jquery.dataTables.min.js",
                          "~/vendor/datatables.net-bs/js/dataTables.bootstrap.min.js"
                          ));

            bundles.Add(new ScriptBundle("~/bundles/vendor7").Include(
                           "~/vendor/pdfmake/build/pdfmake.min.js",
                           "~/vendor/pdfmake/build/vfs_fonts.js",
                           "~/vendor/datatables.net-buttons/js/buttons.html5.min.js",
                           "~/vendor/datatables.net-buttons/js/buttons.print.min.js",
                           "~/vendor/datatables.net-buttons/js/dataTables.buttons.min.js",
                           "~/vendor/datatables.net-buttons-bs/js/buttons.bootstrap.min.js"
                          
                           ));

            bundles.Add(new ScriptBundle("~/bundles/vendor8").Include(
                         "~/vendor/xeditable/bootstrap3-editable/js/bootstrap-editable.min.js",
                         "~/vendor/select2-3.5.2/select2.min.js"
                         ));


            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                            "~/scripts/homer.js",
                            "~/scripts/charts.js"
                            ));
        }
    }
}