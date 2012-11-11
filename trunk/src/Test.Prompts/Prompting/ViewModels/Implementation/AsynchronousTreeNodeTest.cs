using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Prompts.Prompting.ViewModels;
using Prompts.Prompting.ViewModels.Implementation;
using Prompts.Service.ReportExecution;
using Test.Prompts.Infrastructure;
using Test.Prompts.Infrastructure.Fakes;

namespace Test.Prompts.Prompting.ViewModels.Implementation
{
    [TestClass]
    public class AsynchronousTreeNodeTest
    {
        private const string PromptName = "Prompt Name";
        private const string ParameterName = "Parameter Name";
        private AsynchronousTreeNode _asynchronousTreeNode;
        private ValidValue _validValue;
        private FakeChildTreeNodeService _fakeChildTreeNodeService;

        [TestInitialize]
        public void Setup()
        {
            _validValue = A.ValidValue().WithLabel("Label").WithValue("Value").Build();
            _fakeChildTreeNodeService = new FakeChildTreeNodeService();
            _asynchronousTreeNode = new AsynchronousTreeNode(
                PromptName
                , ParameterName
                , _validValue
                , _fakeChildTreeNodeService.Object
                , null);
        }

        [TestMethod]
        public void ItIsConstructedWithALoadingTreeNodeForChildren()
        {
            Assert.IsInstanceOfType(_asynchronousTreeNode.Children.AssertSingle(), typeof(LoadingTreeNode));
        }

        [TestMethod]
        public void ItCallsTheChildTreeNodeServiceWhenExpandedAndSetsTheChildrenToResultOfItsCallback()
        {
            const string promptName = "Prompt Name";
            const string parameterName = "Parameter Name";

            var childrenToReturn = A.ObservableCollection(Mock.Of<ITreeNode>());

            _fakeChildTreeNodeService.SetupGetChildren(promptName, parameterName, _asynchronousTreeNode);

            _asynchronousTreeNode.IsExpanded = true;

            _fakeChildTreeNodeService.AssertNumberOfGetChildren(
                promptName
                , parameterName
                , _asynchronousTreeNode
                , Times.Exactly(1));

            Assert.IsInstanceOfType(_asynchronousTreeNode.Children.AssertSingle(), typeof(LoadingTreeNode));

            _fakeChildTreeNodeService.ExecuteGetChildrenCallback(childrenToReturn);

            Assert.AreEqual(childrenToReturn, _asynchronousTreeNode.Children);
        }

        [TestMethod]
        public void ItSetsTheChildrenToANodeWithTheErrorWhenTheServiceRaisesAnErrorEvent()
        {
            const string errorMessage = "Error Message";
            const string promptName = "Prompt Name";
            const string parameterName = "Parameter Name";

            _fakeChildTreeNodeService.SetupGetChildren(promptName, parameterName, _asynchronousTreeNode);

            _asynchronousTreeNode.IsExpanded = true;

            _fakeChildTreeNodeService.RaiseErrorGettingChildTreeNodes("Error Message");

            var errorNode = _asynchronousTreeNode.Children.AssertSingle();
            Assert.IsInstanceOfType(errorNode, typeof(ErrorTreeNode));
            Assert.AreEqual(errorMessage, errorNode.Label);
        }

        [TestMethod]
        public void ItDoesNotRecallTheGetChildrenWhenTheNodeIsReExpanded()
        {
            const string promptName = "Prompt Name";
            const string parameterName = "Parameter Name";

            var childrenToReturn = A.ObservableCollection(Mock.Of<ITreeNode>());

            _fakeChildTreeNodeService.SetupGetChildren(promptName, parameterName, _asynchronousTreeNode);

            _asynchronousTreeNode.IsExpanded = true;

            _fakeChildTreeNodeService.ExecuteGetChildrenCallback(childrenToReturn);

            _asynchronousTreeNode.IsExpanded = false;

            _asynchronousTreeNode.IsExpanded = true;

            _fakeChildTreeNodeService.AssertNumberOfGetChildren(
                promptName
                , parameterName
                , _asynchronousTreeNode
                , Times.Exactly(1));

            Assert.AreEqual(childrenToReturn, _asynchronousTreeNode.Children);
        }

        [TestMethod]
        public void ItIsConstructedWithIsExpandedSetToFalse()
        {
            Assert.IsFalse(_asynchronousTreeNode.IsExpanded);
        }

        [TestMethod]
        public void ItIsConstructedWithTrueForIsEnabled()
        {
            Assert.IsTrue(_asynchronousTreeNode.IsEnabled);
        }

        [TestMethod]
        public void ItRaisesAnEventWhenIsExpandedChangesFromFalseToTrue()
        {
            var numberOfEvents = 0;

            _asynchronousTreeNode.IsExpanded = false;

            _asynchronousTreeNode.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == "IsExpanded")
                    {
                        numberOfEvents++;
                    }
                };

            _asynchronousTreeNode.IsExpanded = true;

            Assert.AreEqual(1, numberOfEvents);
        }

        [TestMethod]
        public void ItRaisesAnEventWhenIsExpandedChangesFromTrueToFalse()
        {
            var numberOfEvents = 0;

            _asynchronousTreeNode.IsExpanded = true;

            _asynchronousTreeNode.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "IsExpanded")
                {
                    numberOfEvents++;
                }
            };

            _asynchronousTreeNode.IsExpanded = false;

            Assert.AreEqual(1, numberOfEvents);
        }
    }
}
