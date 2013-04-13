using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prompts.Prompting.ViewModels.Implementation;
using Prompts.Service.ReportExecution;

namespace Test.Prompts.Prompting.ViewModels.Implementation
{
    [TestClass]
    public class LeafTreeNodeTest
    {
        private ValidValue _validValue;
        private LeafTreeNode _leafTreeNode;

        [TestInitialize]
        public void Setup()
        {
            _validValue = new ValidValue { Label = "Lable", Value = "Value" };
            _leafTreeNode = new LeafTreeNode("Prompt Name", "Parameter Name", _validValue, null);
        }

        [TestMethod]
        public void ItSetsTheChildrenToEmptyCollection()
        {
            Assert.AreEqual(0, _leafTreeNode.Children.Count);
        }

        [TestMethod]
        public void ItIsConstructedWithFalseForIsExpanded()
        {
            Assert.IsFalse(_leafTreeNode.IsExpanded);
        }

        [TestMethod]
        public void ItIsConstructedWithTrueForIsEnabled()
        {
            Assert.IsTrue(_leafTreeNode.IsEnabled);
        }

        [TestMethod]
        public void ItDoesNothingWhenIsExpandedIsSet()
        {
            _leafTreeNode.IsExpanded = true;
            Assert.IsFalse(_leafTreeNode.IsExpanded);
        }
    }
}
