using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Optimization;
using GPSoftware.Web.Optimization.Resources;
using NUglify;

namespace GPSoftware.Web.Optimization {

    /// <summary>
    ///     Bundle transformation that performs JavaScript minification using NUglify.
    /// </summary>
    public class JsMinifyUglify : IBundleTransform {

        public const string JsContentType = "text/javascript";

        public static readonly JsMinifyUglify Instance = new JsMinifyUglify();

        public JsMinifyUglify() {
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
        /// Transforms the bundle contents by applying javascript minification
        /// </summary>
        /// <param name="context">The <see cref="BundleContext"/> object that contains state for both the framework configuration and the HTTP request.</param>
        /// <param name="response">A <see cref="BundleResponse"/> object containing the bundle contents.</param>
        /// <exception cref="ArgumentNullException"><paramref name="context"/> or <paramref name="response"/> is <c>null</c>.</exception>
        public virtual void Process(BundleContext context, BundleResponse response) {
            if (context == null) {
                throw new ArgumentNullException(nameof(context));
            }
            if (response == null) {
                throw new ArgumentNullException(nameof(response));
            }

            // Don't minify in Instrumentation mode
            if (context.EnableOptimizations && !context.EnableInstrumentation) {
                var result = Uglify.Js(response.Content);
                response.Content = !result.HasErrors ? result.Code : GenerateErrorResponse(response, result.Errors);
            }

            response.ContentType = JsContentType;
        }
    }
}