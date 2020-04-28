using System.ComponentModel;
using System.Web.Optimization;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;

namespace GPSoftware.Web.Optimization {

    /// <summary>
    /// Hooks up the <see cref="BundleModule"/> using the Microsoft.Web.Infrastructure library.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class PreApplicationStartCode {
        private static bool _startWasCalled;

        /// <summary>
        /// Hooks up the BundleModule
        /// </summary>
        public static void Start() {
            // Even though ASP.NET will only call each PreAppStart once, we sometimes internally call one PreAppStart from 
            // another PreAppStart to ensure that things get initialized in the right order. ASP.NET does not guarantee the 
            // order so we have to guard against multiple calls.
            // All Start calls are made on same thread, so no lock needed here.

            if (_startWasCalled) {
                return;
            }
            _startWasCalled = true;

            // Need to register the BundleModule dynamically
            DynamicModuleUtility.RegisterModule(typeof(BundleModule));
        }
    }
}
