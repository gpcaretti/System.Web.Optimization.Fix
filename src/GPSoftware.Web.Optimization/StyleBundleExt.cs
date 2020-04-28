using System;
using System.Linq;
using System.Web;

namespace GPSoftware.Web.Optimization {

    /// <summary>
    ///     StyleBundle Extension methods
    /// </summary>
    public static class StyleBundleExt {

        /// <summary>
        ///     Include a stylesheet to fallback to when external CdnPath does not load.
        /// </summary>
        /// <param name="bundle"></param>
        /// <param name="fallback">Virtual path to fallback stylesheet</param>
        /// <param name="className">Stylesheet class name applied to test DOM element</param>
        /// <param name="ruleName">Rule name to test when the class is applied ie. width</param>
        /// <param name="ruleValue">Value to test when the class is applied ie. 1px</param>
        /// <see cref="https://github.com/EmberConsultingGroup/StyleBundleFallback"/>
        public static StyleBundle IncludeFallback(this StyleBundle bundle, string fallback,
            string className = null, string ruleName = null, string ruleValue = null) {

            return (StyleBundle)bundle.DoIncludeFallback(fallback, className, ruleName, ruleValue);
        }

        /// <summary>
        ///     Include a stylesheet to fallback to when external CdnPath does not load.
        /// </summary>
        /// <param name="bundle"></param>
        /// <param name="fallback">Virtual path to fallback stylesheet</param>
        /// <param name="className">Stylesheet class name applied to test DOM element</param>
        /// <param name="ruleName">Rule name to test when the class is applied ie. width</param>
        /// <param name="ruleValue">Value to test when the class is applied ie. 1px</param>
        /// <see cref="https://github.com/EmberConsultingGroup/StyleBundleFallback"/>
        public static StyleBundle IncludeFallback(this System.Web.Optimization.StyleBundle bundle, string fallback,
            string className = null, string ruleName = null, string ruleValue = null) {

            return (StyleBundle)bundle.DoIncludeFallback(fallback, className, ruleName, ruleValue);
        }

        private static System.Web.Optimization.Bundle DoIncludeFallback(this System.Web.Optimization.Bundle bundle, string fallback,
            string className = null, string ruleName = null, string ruleValue = null) {

            if (!(bundle is StyleBundle) && (bundle is System.Web.Optimization.StyleBundle)) {
                throw new ArgumentException("Not allowed type", nameof(bundle));
            }

            if (String.IsNullOrEmpty(bundle.CdnPath)) {
                throw new ArgumentException("CdnPath must be provided when specifying a fallback", nameof(fallback));
            }

            if (VirtualPathUtility.IsAppRelative(bundle.CdnPath)) {
                bundle.DoCdnFallbackExpress(fallback);
            } else if (new[] { className, ruleName, ruleValue }.Any(String.IsNullOrEmpty)) {
                throw new Exception(
                    "IncludeFallback for cross-domain CdnPath must provide values for parameters [className, ruleName, ruleValue].");
            } else {
                bundle.DoCdnFallbackExpress(fallback, className, ruleName, ruleValue);
            }

            return bundle;
        }

        private static System.Web.Optimization.Bundle DoCdnFallbackExpress(this System.Web.Optimization.Bundle bundle, string fallback,
            string className = null, string ruleName = null, string ruleValue = null) {
            bundle.Include(fallback);

            fallback = VirtualPathUtility.ToAbsolute(fallback);

            bundle.CdnFallbackExpression = String.IsNullOrEmpty(className)
                
                ? String.Format(@"function() {{
                    var len = document.styleSheets.length;
                    for (var i = 0; i < len; i++) {{
                        var sheet = document.styleSheets[i];
                        if (sheet.href && sheet.href.indexOf('{0}') !== -1) {{
                            var rules = sheet.rules || sheet.cssRules;
                            if (rules.length <= 0) {{
                                document.write('<link href=""{1}"" rel=""stylesheet"" type=""text/css"" />');
                            }}
                        }}
                    }}
                    return true;
                    }}()", bundle.CdnPath, fallback)
                
                : String.Format(@"function() {{
                    var loadFallback,
                        len = document.styleSheets.length;
                    for (var i = 0; i < len; i++) {{
                        var sheet = document.styleSheets[i];
                        if (sheet.href && sheet.href.indexOf('{0}') !== -1) {{
                            var meta = document.createElement('meta');
                            meta.className = '{2}';
                            document.head.appendChild(meta);
                            var value = window.getComputedStyle(meta).getPropertyValue('{3}');
                            document.head.removeChild(meta);
                            if (value !== '{4}') {{
                                document.write('<link href=""{1}"" rel=""stylesheet"" type=""text/css"" />');
                            }}
                        }}
                    }}
                return true;
            }}()", bundle.CdnPath, fallback, className, ruleName, ruleValue);

            return bundle;
        }
    }

}
