using System.Web;
using System.Web.Optimization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GPsoftware.Web.Optimization.Test {

    [TestClass]
    public class JsMinifyTest {
        [TestMethod]
        public void JsMinifyDoesNotMinifyInInstrumentationModeTest() {
            string js = "foo = bar;\r\nfoo = yes;";
            JsMinifyUglify jsmin = new JsMinifyUglify();
            BundleContext context = new BundleContext(
                    new Mock<HttpContextBase>().Object,
                    new Mock<BundleCollection>().Object,
                    "/test/virtualpath");
            context.EnableInstrumentation = true;
            BundleResponse response = new BundleResponse(js, null);
            response.Content = js;
            jsmin.Process(context, response);
            Assert.AreEqual(js, response.Content);
        }

        [TestMethod]
        public void JsMinifyRemovesCommentsNewLinesAndSpacesTest() {
            string js = "//I am a comment\r\nfoo = bar;\r\nfoo = yes;";
            JsMinifyUglify jsmin = new JsMinifyUglify();
            BundleContext context = new BundleContext(
                    new Mock<HttpContextBase>().Object,
                    new Mock<BundleCollection>().Object,
                    "/test/virtualpath");
            BundleResponse response = new BundleResponse(js, null);
            response.Content = js;
            jsmin.Process(context, response);
            Assert.AreEqual("foo=bar;foo=yes", response.Content);
        }

        [TestMethod]
        public void JsMinifyKeepsImportantCommentsTest() {
            string js = "/*!I am important */";
            JsMinifyUglify jsmin = new JsMinifyUglify();
            BundleContext context = new BundleContext(
                    new Mock<HttpContextBase>().Object,
                    new Mock<BundleCollection>().Object,
                    "/test/virtualpath");
            BundleResponse response = new BundleResponse(js, null);
            response.Content = js;
            jsmin.Process(context, response);
            Assert.IsTrue(response.Content.StartsWith(js));
        }

        [TestMethod]
        public void JsMinifyContentTypeTest() {
            // Just to have a test failure if we change this header
            Assert.AreEqual("text/javascript", JsMinifyUglify.JsContentType);
        }

        [TestMethod]
        public void JsMinifyDoesNotRenameEvalMethods() {
            // Based of WebFormsUIValidation.js eval usage
            string js = @"function ValidatorOnLoad() {
    var i, val;
    for (i = 0; i < 10; i++) {
        val = i;
        eval(""val.evaluationfunction = "" + val.evaluationfunction + "";"");
    }
}";
            JsMinifyUglify jsmin = new JsMinifyUglify();
            BundleContext context = new BundleContext(
                    new Mock<HttpContextBase>().Object,
                    new Mock<BundleCollection>().Object,
                    "/test/virtualpath");
            BundleResponse response = new BundleResponse(js, null);
            response.Content = js;
            jsmin.Process(context, response);
            Assert.IsTrue(response.Content.Contains("eval(\"val.evaluationfunction"));
        }
    }
}
