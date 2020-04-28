using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Web.Optimization.Fix.Test {

    [TestClass]
    public class BundleResolverTest {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void EnsureNonVirtualPathsDoNotThrowTest() {
            BundleCollection col = new BundleCollection();
            BundleResolver resolver = new BundleResolver(col);
            Assert.IsFalse(resolver.IsBundleVirtualPath("missingTilde"));
            Assert.IsNull(resolver.GetBundleContents("missingTilde"));
            Assert.IsNull(resolver.GetBundleUrl("missingTilde"));
        }

        [TestMethod]
        public void EnsureNullVirtualPathsDoNotThrowTest() {
            BundleCollection col = new BundleCollection();
            BundleResolver resolver = new BundleResolver(col);
            Assert.IsFalse(resolver.IsBundleVirtualPath(null));
            Assert.IsNull(resolver.GetBundleContents(null));
            Assert.IsNull(resolver.GetBundleUrl(null));
        }

        [TestMethod]
        public void EnsureEmptyVirtualPathsDoNotThrowTest() {
            BundleCollection col = new BundleCollection();
            BundleResolver resolver = new BundleResolver(col);
            Assert.IsFalse(resolver.IsBundleVirtualPath(String.Empty));
            Assert.IsNull(resolver.GetBundleContents(String.Empty));
            Assert.IsNull(resolver.GetBundleUrl(String.Empty));
        }

        [TestMethod]
        public void NonBundleValidUrlTest() {
            BundleCollection col = new BundleCollection();
            col.Add(new Bundle("~/js"));
            BundleResolver resolver = new BundleResolver(col);
            Assert.IsFalse(resolver.IsBundleVirtualPath("~/nope"));
            Assert.IsNull(resolver.GetBundleContents("~/nope"));
            Assert.IsNull(resolver.GetBundleUrl("~/nope"));
        }

        [TestMethod]
        public void ValidBundleUrlTest() {
            BundleCollection col = new BundleCollection();
            col.Add(new Bundle("~/js"));
            BundleResolver resolver = new BundleResolver(col);
            Assert.IsTrue(resolver.IsBundleVirtualPath("~/js"));
        }

    }
}
