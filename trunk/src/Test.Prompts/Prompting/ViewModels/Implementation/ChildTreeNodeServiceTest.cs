using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Prompts.Prompting.Construction;
using Prompts.Prompting.ViewModels;
using Prompts.Prompting.ViewModels.Implementation;
using Test.Prompts.Infrastructure;
using Test.Prompts.Infrastructure.Fakes;

namespace Test.Prompts.Prompting.ViewModels.Implementation
{
    [TestClass]
    public class ChildTreeNodeServiceTest
    {
        private Mock<ITreeNodeCollectionBuilder> _treeNodeCollectionBuilder;
        private ChildTreeNodeService _treeNodeService;
        private FakeChildPromptLevelServiceClient _fakeChildPromptLevelServiceClient;

        [TestInitialize]
        public void Setup()
        {
            _fakeChildPromptLevelServiceClient = new FakeChildPromptLevelServiceClient();
            _treeNodeCollectionBuilder = new Mock<ITreeNodeCollectionBuilder>();
            _treeNodeService = new ChildTreeNodeService(
                _fakeChildPromptLevelServiceClient.Object,
                _treeNodeCollectionBuilder.Object);
        }

        [TestMethod]
        public void ItCallsBackWithTheNodesFromTheBuilder()
        {
            var numberOfCallbacks = 0;

            const string promptName = "Prompt Name";
            const string parameterName = "Parameter Name";
            var parameterValues = A.ObservableCollection(A.ParameterValue().Build(), A.ParameterValue().Build());
            var availableItems = A.Array(A.ValidValue().Build(), A.ValidValue().Build());
            var promptLevel = A.PromptLevel().WithAvailableItems(availableItems).HasChildLevel(true).WithParameterName("Prompt Level Parameter Name").Build();
            var treeNodes = A.ObservableCollection(Mock.Of<ITreeNode>(), Mock.Of<ITreeNode>());

            var treeNode = Mock.Of<ITreeNode>(n => n.GetParameterValueHierarchy() == parameterValues);

            _treeNodeCollectionBuilder.Setup(
                b =>
                b.BuildRegularNodesFrom(promptName, promptLevel.ParameterName, availableItems, treeNode, promptLevel.HasChildLevel))
                .Returns(treeNodes);

            _fakeChildPromptLevelServiceClient.SetupGetChildren(promptName, parameterName, parameterValues);

            _treeNodeService.GetChildrenFor(promptName, parameterName, treeNode, n =>
                {
                    numberOfCallbacks++;
                    Assert.AreEqual(treeNodes, n);
                });

            Assert.AreEqual(0, numberOfCallbacks);

            _fakeChildPromptLevelServiceClient.RaiseGetChildrenCompleted(promptLevel);

            Assert.AreEqual(1, numberOfCallbacks);
        }

        [TestMethod]
        public void ItRaisesAnEventWhenTheServiceResponseContainsAnError()
        {
            var numberOfErrorEvents = 0;
            const string errorMessage = "Error Message";

            _treeNodeService.ErrorGettingChildTreeNodes += (s, e) =>
                {
                    numberOfErrorEvents++;
                    Assert.AreEqual(errorMessage, e.ErrorMessage);
                };

            var numberOfCallbacks = 0;
            
            const string promptName = "Prompt Name";
            const string parameterName = "Parameter Name";
            var parameterValues = A.ObservableCollection(A.ParameterValue().Build(), A.ParameterValue().Build());

            var treeNode = Mock.Of<ITreeNode>(n => n.GetParameterValueHierarchy() == parameterValues);

            _fakeChildPromptLevelServiceClient.SetupGetChildren(promptName, parameterName, parameterValues);

            _treeNodeService.GetChildrenFor(promptName, parameterName, treeNode, n =>
            {
                numberOfCallbacks++;
            });

            Assert.AreEqual(0, numberOfCallbacks);

            _fakeChildPromptLevelServiceClient.RaiseGetChildrenError(errorMessage);

            Assert.AreEqual(0, numberOfCallbacks);
            Assert.AreEqual(1, numberOfErrorEvents);
        }
    }
}