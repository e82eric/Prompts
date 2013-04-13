using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Prompts.Prompting.Construction;
using Prompts.Prompting.ViewModels.Implementation;
using Test.Prompts.Infrastructure.Fakes;

namespace Test.Prompts.Prompting.ViewModels.Implementation
{
    [TestClass]
    public class ChildPromptItemServiceTest
    {
        private Mock<IPromptItemCollectionBuilder> _promptItemCollectionBuilder;
        private ChildPromptItemsService _childPromptItemsService;
        private FakeChildPromptLevelServiceClient _fakePromptLevelServiceClient;

        [TestInitialize]
        public void Setup()
        {
            _fakePromptLevelServiceClient = new FakeChildPromptLevelServiceClient();
            _promptItemCollectionBuilder = new Mock<IPromptItemCollectionBuilder>();
            _childPromptItemsService = new ChildPromptItemsService(
                _fakePromptLevelServiceClient.Object,
                _promptItemCollectionBuilder.Object);
        }

        [TestMethod]
        public void ItUsesTheErrorCallbacktWhenTheCallbackContainsAnError()
        {
            const string errorMessage = "Error Message";
            var numberOfErrorEvents = 0;

            var numberOfChildPromptItemEvents = 0;

            const string promptName = "Prompt Name";
            const string parameterName = "Parameter Name";
            const string value = "Value";

            _fakePromptLevelServiceClient.SetupGetChildren2(
                promptName,
                parameterName,
                value);

            _childPromptItemsService.GetChildren(
                promptName,
                parameterName,
                value,
                c =>
                    {
                        numberOfChildPromptItemEvents++;
                    },
                e =>
                    {
                        numberOfErrorEvents++;
                        Assert.AreEqual(errorMessage, e);
                    });

            Assert.AreEqual(0, numberOfChildPromptItemEvents);

            _fakePromptLevelServiceClient.RaiseGetChildrenError(errorMessage);

            Assert.AreEqual(0, numberOfChildPromptItemEvents);
            Assert.AreEqual(1, numberOfErrorEvents);
        }
    }
}
