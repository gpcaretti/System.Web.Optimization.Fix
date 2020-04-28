using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Optimization;
using GPsoftware.Web.Optimization.Resources;
using NUglify;

namespace GPsoftware.Web.Optimization {

    /// <summary>
    /// Bundle transformation that performs CSS minification.
    /// </summary>
    public class CssMinifyUglify : IBundleTransform {

        public const string CssContentType = "text/css";

        public static readonly CssMinifyUglify Instance = new CssMinifyUglify();

        public CssMinifyUglify() {
        }

        protected static string GenerateErrorResponse(BundleResponse bundle, IEnumerable<object> errors) {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("/* ");
            stringBuilder.Append(OptimizationResources.MinifyError).Append(Environment.NewLine);
            foreach (object error in errors) {
                stringBuilder.Append(error.ToString()).Append(Environment.NewLine);
            }
            stringBuilder.Append(" */" + Environment.NewLine);
            stringBuilder.Append(bundle.Content);
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Minifies the supplied CSS bundle and sets the Http content-type header to 'text/css'
        /// </summary>
        /// <param name="context">The <see cref="BundleContext"/> object that contains state for both the framework configuration and the HTTP request.</param>
        /// <param name="response">A <see cref="BundleResponse"/> object containing the bundle contents.</param>
        public virtual void Process(BundleContext context, BundleResponse response) {
            if (context == null) {
                throw new ArgumentNullException(nameof(context));
            }
            if (response == null) {
                throw new ArgumentNullException(nameof(response));
            }

            // Don't minify in Instrumentation mode
            if (context.EnableOptimizations && !context.EnableInstrumentation) {
                var result = Uglify.Css(response.Content);
                response.Content = !result.HasErrors ? result.Code : GenerateErrorResponse(response, result.Errors);
            }

            response.ContentType = CssContentType;
        }
    }
}