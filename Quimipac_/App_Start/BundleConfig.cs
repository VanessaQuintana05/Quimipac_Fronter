using System.Web;
using System.Web.Optimization;

namespace Quimipac_
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            // para la producción, use la herramienta de compilación disponible en https://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));


            bundles.Add(new StyleBundle("~/Css").Include(
                   "~/Content/bower_components/bootstrap/dist/css/bootstrap.min.css",
                       "~/Content/bower_components/font-awesome/css/font-awesome.min.css",
                       "~/Content/bower_components/Ionicons/css/ionicons.min.css",
                       "~/Content/dist/css/AdminLTE.min.css",
                       "~/Content/dist/css/skins/_all-skins.min.css",
                       "~/Content/plugins/iCheck/square/blue.css"
                       ));

            bundles.Add(new ScriptBundle("~/Js").Include(
                   "~/Content/bower_components/jquery/dist/jquery.min.js",
                   "~/Content/bower_components/bootstrap/dist/js/bootstrap.min.js",
                   "~/Content/bower_components/jquery-slimscroll/jquery.slimscroll.min.js",
                   "~/Content/bower_components/fastclick/lib/fastclick.js",
                   "~/Content/dist/js/adminlte.min.js"
                   ));
        }
    }
}
