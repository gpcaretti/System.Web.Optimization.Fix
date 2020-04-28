using System.Web.Optimization;

namespace GPsoftware.Web.Optimization.Test {
    public class UppercaseTransform : IItemTransform {
        public string Process(string itemVirtualPath, string input) {
            return input.ToUpperInvariant();
        }
    }

    public class AppendTransform : IBundleTransform {
        private string _txt;
        public AppendTransform(string txt) {
            _txt = txt;
        }

        public void Process(BundleContext context, BundleResponse response) {
            response.Content += _txt;
        }
    }
}
