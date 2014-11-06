using System.Web;
using System.Web.Optimization;

namespace CalculatorZd
{
    public class BundleConfig
    {
       public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js","~/Scripts/jquery.json-2.4.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-ui").Include(
                       "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap/js").Include("~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrap-select.js"));

            bundles.Add(new StyleBundle("~/bundles/bootstrap-select/css").Include("~/Content/bootstrap/bootstrap-select.css"));

          
           bundles.Add(new StyleBundle("~/bundles/bootstrap/css").Include("~/Content/bootstrap/bootstrap.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/bootstrap-responsive.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                      //  "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));

            bundles.Add(new ScriptBundle("~/bundles/autocomplete").Include(
                        "~/Scripts/autocomplete.js"));

            bundles.Add(new ScriptBundle("~/bundles/datepicker").Include(
                     "~/Scripts/datepicker.js"));

          bundles.Add(new ScriptBundle("~/bundles/ngBase").Include(
                "~/Scripts/vendor/angular.js","~/app/ng/app.js"));// "~/Scripts/vendor/jquery.js",

           bundles.Add(new ScriptBundle("~/bundles/dataservice").Include("~/app/ng/services/dataService.js"));

          bundles.Add(new ScriptBundle("~/bundles/filterSettings").Include(
               "~/app/ng/controllers/filterSettings.js", "~/Scripts/jquery.json-2.4.js"));

            bundles.Add(new ScriptBundle("~/bundles/calculator").Include(
               "~/app/ng/controllers/calculator.js",
               "~/app/ng/services/dataService.js", "~/Scripts/jquery.json-2.4.js"));//

            bundles.Add(new ScriptBundle("~/bundles/ui-bootstrap-tpls").Include("~/Scripts/ui-bootstrap-tpls-0.10.0.min.js"));//
           

            bundles.Add(new ScriptBundle("~/bundles/autocomplete").Include(
               "~/app/ng/controllers/autocomplete.js"));
            //bundles.Add(new ScriptBundle("~/bundles/multiselect").Include(
            //   "~/app/ng/controllers/multiselect.js", "~/Scripts/multiselect.js"));
        }
    }
}
