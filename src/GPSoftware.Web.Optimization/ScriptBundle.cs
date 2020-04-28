
using System;
using System.Web.Optimization;

namespace GPSoftware.Web.Optimization {

    /// <summary>
    /// Bundle designed specifically for processing JavaScript
    /// </summary>
    public class ScriptBundle : Bundle {

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptBundle"/> class.
        /// </summary>
        /// <param name="virtualPath">The virtual path used to reference the <see cref="ScriptBundle"/> from within a view or Web page.</param>
        public ScriptBundle(string virtualPath) : this(virtualPath, null) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptBundle"/> class.
        /// </summary>
        /// <param name="virtualPath">The virtual path used to reference the <see cref="ScriptBundle"/> from within a view or Web page.</param>
        /// <param name="cdnPath">An alternate url for the bundle when it is stored in a content delivery network.</param>
        public ScriptBundle(string virtualPath, string cdnPath)
            : this(virtualPath, cdnPath, JsMinifyUglify.Instance) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptBundle"/> class.
        /// </summary>
        /// <param name="virtualPath">The virtual path used to reference the <see cref="ScriptBundle"/> from within a view or Web page.</param>
        /// <param name="cdnPath">An alternate url for the bundle when it is stored in a content delivery network.</param>
        /// <param name="transforms">A list of System.Web.Optimization.IBundleTransform objects which process the contents of the bundle in the order which they are added.</param>
        public ScriptBundle(string virtualPath, string cdnPath, params IBundleTransform[] transforms)
            : base(virtualPath, cdnPath, transforms) {
            base.ConcatenationToken = ";" + Environment.NewLine;
        }
    }
}
