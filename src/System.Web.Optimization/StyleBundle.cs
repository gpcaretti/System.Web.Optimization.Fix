namespace System.Web.Optimization.Fix {

    /// <summary>
    /// Represents a bundle that does CSS minification.
    /// </summary>
    public class StyleBundle : Bundle {

        /// <summary>
        /// Initializes a new instance of the <see cref="StyleBundle"/> class.
        /// </summary>
        /// <param name="virtualPath">The virtual path used to reference the <see cref="StyleBundle"/> from within a view or Web page.</param>
        public StyleBundle(string virtualPath) : this(virtualPath, null) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StyleBundle"/> class.
        /// </summary>
        /// <param name="virtualPath">The virtual path used to reference the <see cref="StyleBundle"/> from within a view or Web page.</param>
        /// <param name="cdnPath">An alternate url for the bundle when it is stored in a content delivery network.</param>
        public StyleBundle(string virtualPath, string cdnPath)
            : this(virtualPath, cdnPath, CssMinifyUglify.Instance) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StyleBundle"/> class.
        /// </summary>
        /// <param name="virtualPath">The virtual path used to reference the <see cref="StyleBundle"/> from within a view or Web page.</param>
        /// <param name="cdnPath">An alternate url for the bundle when it is stored in a content delivery network.</param>
        /// <param name="transforms">A list of <see cref="System.Web.Optimization.IBundleTransform" /> objects which process the contents of the bundle in the order which they are added.</param>
        public StyleBundle(string virtualPath, string cdnPath, IBundleTransform transforms)
            : base(virtualPath, cdnPath, transforms) {
            base.ConcatenationToken = ";" + Environment.NewLine;
        }
    }
}
