using System.Web;
using System.Web.Optimization;

namespace TalentGoManagerWebApp
{
    public class BundleConfig
    {
        // 有关捆绑的详细信息，请访问 https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // 生产准备就绪，请使用 https://modernizr.com 上的生成工具仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));
            //JQuery UI Scripts and Styles.
            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                "~/Scripts/jquery-ui-{version}.js"
                ));

            bundles.Add(new StyleBundle("~/Content/jqueryuicss").Include(
                "~/Content/themes/base/*.css"
                ));


            //JQuery File Upload Scripts and styles
            bundles.Add(new ScriptBundle("~/bundles/jqueryfileupload").Include(
                "~/Scripts/JQuery.FileUpload/jquery.iframe-transport.js",
                "~/Scripts/JQuery.FileUpload/jquery.fileupload.js"
                ));
            bundles.Add(new StyleBundle("~/Content/jqueryfileupload").Include(
                "~/Content/JQuery.FileUpload/css/jquery.fileupload.css"
                ));

            //JQuery tmpl
            bundles.Add(new ScriptBundle("~/bundles/jquerytmpl").Include(
                "~/Scripts/JQuery.tmpl/jquery.tmpl.js"
                ));

            //Bootstrap-DatetimePicker
            bundles.Add(new ScriptBundle("~/bundles/bootstrapDatetimePicker").Include(
                "~/Scripts/smalot-datetimepicker/bootstrap-datetimepicker.js",
                "~/Scripts/smalot-datetimepicker/locales/bootstrap-datetimepicker.zh-CN.js"
                ));
            bundles.Add(new StyleBundle("~/Content/bootstrapDatetimePicker").Include(
                "~/Content/smalot-datetimepicker/bootstrap-datetimepicker.css"
                ));

            //网站基本样式
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
        }
    }
}
