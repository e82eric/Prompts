using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prompts.Prompting.ViewModels.Implementation;

namespace Test.Prompts.Prompting.ViewModels.Implementation
{
    [TestClass]
    public class ErrorTreeNodeTest
    {
        [TestMethod]
        public void ItUsesLabelForLabel()
        {
            const string label = "Label";

            var errorTreeNode = new ErrorTreeNode(label);
            Assert.AreEqual(label, errorTreeNode.Label);
        }

        [TestMethod]
        public void IsExpandedIsFalse()
        {
            var errorTreeNode = new ErrorTreeNode("Label");
            Assert.IsFalse(errorTreeNode.IsExpanded);
        }

        [TestMethod]
        public void ItIsNotEnabled()
        {
            var errorTreeNode = new ErrorTreeNode("Label");
            Assert.IsFalse(errorTreeNode.IsEnabled);
        }
    }
}
