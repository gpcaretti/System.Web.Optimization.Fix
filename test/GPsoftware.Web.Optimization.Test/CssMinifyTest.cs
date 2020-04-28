using System.Web;
using System.Web.Optimization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GPsoftware.Web.Optimization.Test {

    [TestClass]
    public class CssMinifyTest {
        [TestMethod]
        public void CssMinifyDoesNotMinifyInInstrumentationModeTest() {
            string css = "body\r\n{ }";
            CssMinifyUglify cssmin = new CssMinifyUglify();
            BundleContext context = new BundleContext(
                    new Mock<HttpContextBase>().Object,
                    new Mock<BundleCollection>().Object,
                    "/test/virtualpath");
            context.EnableInstrumentation = true;
            BundleResponse response = new BundleResponse(css, null);
            cssmin.Process(context, response);
            Assert.AreEqual(css, response.Content);
        }

        [TestMethod]
        public void CssMinifyRemovesWhitespaceTest() {
            string css = "body\r\n\t {color : #00f }";
            CssMinifyUglify cssmin = new CssMinifyUglify();
            BundleContext context = new BundleContext(
                    new Mock<HttpContextBase>().Object,
                    new Mock<BundleCollection>().Object,
                    "/test/virtualpath");
            BundleResponse response = new BundleResponse(css, null);
            cssmin.Process(context, response);
            Assert.AreEqual("body{color:#00f}", response.Content);
        }

        [TestMethod]
        public void CssMinifyKeepsImportantCommentsTest() {
            string css = "/*!I am important */";
            CssMinifyUglify cssmin = new CssMinifyUglify();
            BundleContext context = new BundleContext(
                    new Mock<HttpContextBase>().Object,
                    new Mock<BundleCollection>().Object,
                    "/test/virtualpath");
            BundleResponse response = new BundleResponse(css, null);
            cssmin.Process(context, response);
            Assert.IsTrue(response.Content.StartsWith(css));
        }

        [TestMethod]
        public void CssMinifyContentTypeTest() {
            // Just to have a test failure if we change this header
            Assert.AreEqual("text/css", CssMinifyUglify.CssContentType);
        }
    }
}
