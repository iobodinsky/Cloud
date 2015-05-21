using System.Web.Optimization;

namespace Cloud.WebApi
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/common").Include(
                "~/Scripts/assets/js/jquery-1.10.2.min.js",
                "~/Scripts/assets/js/bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/ng").Include(
					 "~/Scripts/assets/js/angular.js",
					 "~/Scripts/assets/js/angular-file-upload.js",
					 "~/Scripts/assets/js/ui-bootstrap-tpls-0.13.0.js",
					 "~/Scripts/app/models/constants.js",
					 "~/Scripts/app/services/userTokenService.js",
					 "~/Scripts/app/controllers/cloudController.js",
					 "~/Scripts/app/controllers/renameFileModalController.js",
                "~/Scripts/app/cloud.js"));

            bundles.Add(new StyleBundle("~/Content/css")
                .IncludeDirectory("~/Content", "*.css", true));

            BundleTable.EnableOptimizations = false;
        }
    }
}