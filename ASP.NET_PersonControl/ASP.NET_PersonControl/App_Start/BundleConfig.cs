using System.Web;
using System.Web.Optimization;

namespace ASP.NET_PersonControl
{
    public class BundleConfig
    {
        // Дополнительные сведения об объединении см. на странице https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/moment.js",
                        "~/Scripts/chart.js",
                         "~/Scripts/dashboard.js",
                          "~/Scripts/maps.js",
                           "~/Scripts/misc.js",
                            "~/Scripts/off-canvas.js",
                            "~/Scripts/jquery-ui-1.12.1.min.j",
                            "~/Scripts/chosen.jquery.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Используйте версию Modernizr для разработчиков, чтобы учиться работать. Когда вы будете готовы перейти к работе,
            // готово к выпуску, используйте средство сборки по адресу https://modernizr.com, чтобы выбрать только необходимые тесты.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrap - datetimepicker.js",
                      "~/Scripts/jquery-ui-1.12.1.min.j",
                      "~/Scripts/chosen.jquery.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Scripts/bootstrap - datetimepicker.css",
                      "~/css/style.css",
                      "~/css/maps/style.css.map",
                      "~/vendors/iconfonts/mdi/css/materialdesignicons.min.css",
                      "~/vendors/css/vendor.bundle.base.css",
                      "~/vendors/css/vendor.bundle.addons.css",
                      "~/images/faces/face1.jpg"
                      ));


           
        }
    }
}
