using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prompts.ReportRendering.Implementation;
using Test.Prompts.Infrastructure;
using Test.Prompts.Infrastructure.Fakes;

namespace Test.Prompts.ReportRendering.Implementation
{
    [TestClass]
    public class ReportExecutionServiceTest
    {
        private ReportExecutionService _reportExecutionService;
        private FakePromptSelectionServiceClient _promptSelectionService;

        [TestInitialize]
        public void Setup()
        {
            _promptSelectionService = new FakePromptSelectionServiceClient();
            _reportExecutionService = new ReportExecutionService(_promptSelectionService.Object);
        }

        [TestMethod]
        public void CallbackWithCorrectExecutionId()
        {
            const string executionIdToCallbackWith = "ExecutionID";

            var numberOfCallbacks = 0;
            var numberOfErrorCallbacks = 0;

            var catalogItem = A.CatalogItemInfo()
                .WithName("Report Name")
                .WithPath("Path")
                .Build();

            var promptSelections = A.ObservableCollection(
                A.PromptSelectionInfo().Build()
                , A.PromptSelectionInfo().Build());

            _promptSelectionService.SetupSetPromptSelections(catalogItem.Path, promptSelections);

            _reportExecutionService.Render(
                catalogItem,
                promptSelections,
                e =>
                    {
                        Assert.AreEqual(executionIdToCallbackWith, e);
                        numberOfCallbacks++;
                    },
                e =>
                    {
                        numberOfErrorCallbacks++;
                    }
                );

            _promptSelectionService.CallbackWith(executionIdToCallbackWith);

            Assert.AreEqual(1, numberOfCallbacks);
            Assert.AreEqual(0, numberOfErrorCallbacks);
        }

        [TestMethod]
        public void ItCallsbackWithTheCorrectErrorMessageWhenTheServiceReportsAnError()
        {
            const string errorMessage = "ExecutionID";

            var numberOfCallbacks = 0;
            var numberOfErrorCallbacks = 0;

            var catalogItem = A.CatalogItemInfo()
                .WithName("Report Name")
                .WithPath("Path")
                .Build();

            var promptSelections = A.ObservableCollection(
                A.PromptSelectionInfo().Build(), 
                A.PromptSelectionInfo().Build());

            _promptSelectionService.SetupSetPromptSelections(catalogItem.Path, promptSelections);

            _reportExecutionService.Render(
                catalogItem,
                promptSelections,
                e =>
                    {
                        numberOfCallbacks++;
                    },
                e =>
                    {
                        Assert.AreEqual(errorMessage, e);
                        numberOfErrorCallbacks++;
                    }
                );

            _promptSelectionService.ErrorCallbackWith(errorMessage);

            Assert.AreEqual(0, numberOfCallbacks);
            Assert.AreEqual(1, numberOfErrorCallbacks);
        }
    }
}
