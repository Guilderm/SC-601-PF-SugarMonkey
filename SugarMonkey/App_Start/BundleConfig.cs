using System.Web.Optimization;

namespace SugarMonkey
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Artifacts/Vendors/JQuery/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Artifacts/Vendors/JQuery/jquery.validate*"));

            bundles.Add(new Bundle("~/SiteWide/Scripts").Include(
                "~/Artifacts/Scripts/SiteWide.js",
                "~/Artifacts/Vendors/Bootstrap 5.0.2/js/bootstrap.bundle.js"));

            bundles.Add(new StyleBundle("~/SiteWide/Style").Include(
                "~/Artifacts/Styles/SiteWide.css",
                "~/Artifacts/Vendors/Bootstrap 5.0.2/css/bootstrap.css"));
        }
    }
}