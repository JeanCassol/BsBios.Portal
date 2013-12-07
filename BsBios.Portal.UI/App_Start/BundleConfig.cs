using System.Web;
using System.Web.Optimization;

namespace BsBios.Portal.UI
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            //para resolver o problema de compatibilidade do kendo UI versão 2012.3.1114 com o 
            //jQuery 1.9.0 foi adicionado o "~/Scripts/jquery-migrate-{version}.js".
            //Quando sair uma nova versão do kendo UI talvez este pacote possa ser removido
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-migrate-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js",
                        "~/Scripts/jquery-ui-i18n.js",
                        "~/Scripts/jquery-ui-extension.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*",
                       "~/Scripts/methods_pt.js"));

            bundles.Add(new ScriptBundle("~/bundles/globalize").Include(
                        "~/Scripts/globalize.js",
                        "~/Scripts/globalize.culture.pt-BR.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/scriptsgerais")
                .Include("~/Scripts/scriptsGerais.js"));

            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
                "~/Scripts/knockout-{version}.js"));


            //bundle não funciona com minified files
            //para fazer funcionar teria que alterar a IgnoreList em bundles.IgnoreList, conforme fórum http://stackoverflow.com/questions/11980458/mvc4-bundler-not-including-min-files
            //bundles.Add(new StyleBundle("~/Content/kendo").Include(
            //    "~/Content/kendo/2012.3.1114/kendo.common.min.css",
            //    "~/Content/kendo/2012.3.1114/kendo.default.min.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css",
                "~/Content/Botoes.css", "~/Content/Formulario.css", "~/Content/Modal.css", "~/Content/AutoComplete.css"));

            bundles.Add(new StyleBundle("~/Content/monitor").Include("~/Content/Monitor.css",
                "~/Content/Botoes.css", "~/Content/Formulario.css", "~/Content/Modal.css", "~/Content/AutoComplete.css"));

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
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));

        }
    }
}