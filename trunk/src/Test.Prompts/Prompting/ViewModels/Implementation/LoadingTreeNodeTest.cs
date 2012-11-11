using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prompts.Prompting.ViewModels.Implementation;

namespace Test.Prompts.Prompting.ViewModels.Implementation
{
    [TestClass]
    public class LoadingTreeNodeTest
    {
        private LoadingTreeNode _loadingTreeNode;

        [TestInitialize]
        public void Setup()
        {
            _loadingTreeNode = new LoadingTreeNode();
        }

        [TestMethod]
        public void ItSetsTheChildrenToAnEmptyCollection()
        {
            Assert.AreEqual(0, _loadingTreeNode.Children.Count);
        }

        [TestMethod]
        public void ItSetsTheLabelToLoadingDotDotDot()
        {
            Assert.AreEqual("Loading...", _loadingTreeNode.Label);   
        }

        [TestMethod]
        public void ItIsConstructedWithFalseForIsEnabled()
        {
            Assert.IsFalse(_loadingTreeNode.IsEnabled);
        }

    }
}
