using System.Web;
using System.Web.Optimization;

namespace PetrolPumpERP
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));

            //// Use the development version of Modernizr to develop with and learn from. Then, when you're
            //// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js",
            //          "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/cssadmin").Include(
                      "~/Content/Admin/bootstrap.min.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/Main/bootstrap.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/Main").Include(
                "~/Scripts/Main/jquery.min.js",
                "~/Scripts/Main/bootstrap.min.js",
                "~/Scripts/Main/angular.min.js",
                "~/Scripts/Main/App.js",
                "~/Scripts/Main/common.js",
                "~/Scripts/Main/spin.js",
                "~/Scripts/Main/ShowCustomAlert.js"
                ));
            bundles.Add(new ScriptBundle("~/bundles/Login").Include("~/Scripts/Main/LoginController.js"));

            bundles.Add(new ScriptBundle("~/bundles/Subledger").Include("~/Scripts/Main/SubledgerController.js"));

            bundles.Add(new ScriptBundle("~/bundles/AccountType").Include("~/Scripts/Main/AccountTypeController.js"));

            bundles.Add(new ScriptBundle("~/bundles/Customer").Include("~/Scripts/Main/CustomerController.js"));

            bundles.Add(new ScriptBundle("~/bundles/Vendor").Include("~/Scripts/Main/VendorController.js"));

            bundles.Add(new ScriptBundle("~/bundles/UOM").Include("~/Scripts/Main/UOMController.js"));

            bundles.Add(new ScriptBundle("~/bundles/ProductType").Include("~/Scripts/Main/ProductTypeController.js"));

            bundles.Add(new ScriptBundle("~/bundles/Product").Include("~/Scripts/Main/ProductController.js"));

            bundles.Add(new ScriptBundle("~/bundles/Opening").Include("~/Scripts/Main/OpeningController.js"));

            bundles.Add(new ScriptBundle("~/bundles/WareHouse").Include("~/Scripts/Main/WareHouseController.js"));

            bundles.Add(new ScriptBundle("~/bundles/Bank").Include("~/Scripts/Main/BanksController.js"));

            bundles.Add(new ScriptBundle("~/bundles/OtherAccount").Include("~/Scripts/Main/OtherAccountController.js"));

            bundles.Add(new ScriptBundle("~/bundles/Sales").Include("~/Scripts/Main/SalesController.js"));

            bundles.Add(new ScriptBundle("~/bundles/Purchase").Include("~/Scripts/Main/PurchaseController.js"));



        }
    }
}
