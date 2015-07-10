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
                "~/Scripts/assets/js/angular-ui-router.min.js",
                "~/Scripts/assets/js/angular-file-upload.js",
                "~/Scripts/assets/js/ui-bootstrap-tpls-0.13.0.js",
                "~/Scripts/app/shared/constants.js",
                "~/Scripts/app/shared/httpService.js",
                "~/Scripts/app/shared/userTokenService.js",
                "~/Scripts/app/shared/alert/alertService.js",
                "~/Scripts/app/shared/alert/alertDirective.js",
                "~/Scripts/app/shared/loader/loaderDirective.js",
                "~/Scripts/app/shared/loader/loaderService.js",
                "~/Scripts/app/components/login/loginController.js",
                "~/Scripts/app/components/register/registerController.js",
                "~/Scripts/app/components/folder/folderController.js",
                "~/Scripts/app/controllers/userAccountController.js",
                "~/Scripts/app/controllers/createFolderModalController.js",
                "~/Scripts/app/components/modals/deleteConfirm/deleteConfirmController.js",
                "~/Scripts/app/components/modals/rename/renameController.js",
                "~/Scripts/app/components/modals/manageStorages/manageStoragesController.js",
                "~/Scripts/app/cloud.routes.js",
                "~/Scripts/app/cloud.module.js"));

            bundles.Add(new StyleBundle("~/Content/css")
                .IncludeDirectory("~/Content", "*.css", true));

            BundleTable.EnableOptimizations = false;
        }
    }
}