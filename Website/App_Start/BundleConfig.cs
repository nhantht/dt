using System.Web;
using System.Web.Optimization;

namespace Website
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.unobtrusive - ajax.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/docs.min.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/boostrap-formhelpers.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/docs.min.css",
                      "~/Content/site.css"));

            //Plugins
            bundles.Add(new ScriptBundle("~/bundles/plugins").Include(
                      "~/Scripts/jquery-ui.js", 
                      "~/Scripts/moment.js",
                      "~/Scripts/datetime.picker.js",
                      "~/Scripts/intlTelInput.min.js",
                      "~/Scripts/utils.js"));

            bundles.Add(new StyleBundle("~/Content/plugins").Include(
                      "~/Content/jquery-ui.css",
                      "~/Content/intlTelInput.css"
                      ));

            //App
            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                      "~/Scripts/validator.js", 
                      "~/Scripts/user.js",
                      "~/Scripts/adv.js",
                      "~/Scripts/index.js",
                      "~/Scripts/campaign.js",
                      "~/Scripts/url.js",
                      "~/Scripts/keyword.js",
                      "~/Scripts/keyworddetail.js",
                      "~/Scripts/category.js"));

            bundles.Add(new StyleBundle("~/Content/app").Include(
                      "~/Content/home.css",
                      "~/Content/header.css",
                      "~/Content/footer.css",
                      "~/Content/loading.css",
                      "~/Content/register.css",
                      "~/Content/adv.css",
                      "~/Content/campaign.css"));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = false;
        }
    }
}
