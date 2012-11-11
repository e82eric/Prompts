using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Prompts.Prompting.ViewModels;
using Prompts.Prompting.ViewModels.Implementation;
using Prompts.Service.ReportExecution;

namespace Test.Prompts.Prompting.ViewModels.Implementation
{
    [TestClass]
    public class RootTreeNodeTest
    {
        private const string PromptName = "Prompt Name";
        private const string ParameterName = "Parameter Name";
        private RootAsynchronousTreeNode _rootAsynchronousTreeNode;
        private Mock<IChildTreeNodeService> _childTreeNodeService;
        private ValidValue _validValue;

        [TestInitialize]
        public void Setup()
        {
            _validValue = new ValidValue {Label = "Label", Value = "Value"};
            _childTreeNodeService = new Mock<IChildTreeNodeService>();
            _rootAsynchronousTreeNode = new RootAsynchronousTreeNode(PromptName, ParameterName, _validValue, _childTreeNodeService.Object);
        }

        [TestMethod]
        public void ItIsConstructedWithAParentOfNull()
        {
            Assert.IsNull(_rootAsynchronousTreeNode.Parent);
        }
    }
}
