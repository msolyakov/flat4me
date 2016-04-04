using System.Collections.Generic;
using System.Web.Optimization;

namespace Flat4Me.Web.App_Start
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            var styleBundle = new StyleBundle("~/content/css")
                // Font-Awesome
                .Include("~/Content/font-awesome/4.3.0/css/font-awesome.css", new CssRewriteUrlTransform())
                // Bootstrap
                .Include("~/Content/bootstrap/3.3.5/css/bootstrap.css", new CssRewriteUrlTransform())
                .Include("~/Content/bootstrap/3.3.5/css/bootstrap-theme.css")
                // Bootstrap Datepicker
                .Include("~/Content/bootstrap-datepicker/1.5.0/css/bootstrap-datepicker3.css")
                // Bootstrap Switch
                .Include("~/Content/bootstrap-switch/3.3.2/css/bootstrap-switch.css")
                // noUIslider
                .Include("~/Content/noUIslider/8.0.1/css/nouislider.css")
                .Include("~/Content/noUIslider/8.0.1/css/nouislider.pips.css")
                // Toast
                .Include("~/Content/toastr/2.1.1/css/toastr.css")
                // Blueimp-gallery
                .Include("~/Content/blueimp-gallery/2.15.2/css/blueimp-gallery.css", new CssRewriteUrlTransform())
                // La Casa
                .Include("~/Content/la-casa/css/reset.css")
                .Include("~/Content/la-casa/css/responsive.css", new CssRewriteUrlTransform())
                // Site
                .Include("~/Content/f4me/css/site.css");

            styleBundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(styleBundle);

            var scriptBundle = new ScriptBundle("~/bundles/js")
                // jQuery
                .Include("~/Content/jquery/2.1.4/js/jquery.js")
                // Bootstrap
                .Include("~/Content/bootstrap/3.3.5/js/bootstrap.js")
                // Bootstrap Datepicker
                .Include("~/Content/bootstrap-datepicker/1.5.0/js/bootstrap-datepicker.js")
                .Include("~/Content/bootstrap-datepicker/1.5.0/js/bootstrap-datepicker.ru.min.js")
                // Bootstrap switch
                .Include("~/Content/bootstrap-switch/3.3.2/js/bootstrap-switch.js")
                // noUIslider
                .Include("~/Content/noUIslider/8.0.1/js/nouislider.js")
                // Toastr
                .Include("~/Content/toastr/2.1.1/js/toastr.js")
                // Blueimp-gallery
                .Include("~/Content/blueimp-gallery/2.15.2/js/blueimp-gallery.js")
                // Typeahead
                .Include("~/Content/typeahead/0.11.1/js/typeahead.bundle.js")
                // Handlebars
                .Include("~/Content/handlebars/3.0.3/js/handlebars.runtime.js")
                // Modernizr
                .Include("~/Content/modernizr/2.8.3/js/modernizr.js")
                // F4Me
                .Include("~/Content/f4me/js/blockUI.js")
                .Include("~/Content/f4me/js/ajaxErrorHandler.js")
                .Include("~/Content/f4me/js/typeahead.js")
                .Include("~/Content/f4me/js/f4me.js")
                .Include("~/Content/f4me/js/accommodation.js")
                .Include("~/Content/f4me/js/map.js")
                .Include("~/Content/f4me/js/user.js")
                .Include("~/Content/f4me/js/init.js")
                // Compiled Handlebars templates
                .Include("~/Content/handlebars/ui-templates/compiled/flatMainListTemplate.js")
                .Include("~/Content/handlebars/ui-templates/compiled/flatTemplateBody.js")
                .Include("~/Content/handlebars/ui-templates/compiled/flatTemplateTop.js")
                .Include("~/Content/handlebars/ui-templates/compiled/flatPriceItemTemplate.js")
                .Include("~/Content/handlebars/ui-templates/compiled/flatTopMenuTemplate.js");

            scriptBundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(scriptBundle);
        }
    }

    class NonOrderingBundleOrderer : IBundleOrderer
    {
        public IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
        {
            return files;
        }
    }
}