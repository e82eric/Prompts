using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prompts.Infastructure;
using Prompts.ReportRendering.ViewModel;
using Test.Prompts.Infrastructure;
using Test.Prompts.Infrastructure.Fakes;

namespace Test.Prompts.ReportRendering.ViewModels
{
    [TestClass]
    public class ReportViewModelTest
    {
        [TestMethod]
        public void GetsCorrectUrlFromServer()
        {
            var numberOfStateEvents = 0;
            var numberOfUrlEvents = 0;
            const string executionIdToCallbackWith = "ExecutionId";

            var promptSelections = A.ObservableCollection(
                A.PromptSelectionInfo().Build(),
                A.PromptSelectionInfo().Build());

            var catalogItemInfo = A.CatalogItemInfo()
                .WithName("Report Name")
                .WithPath("Report Path")
                .Build();

            var fakeReportExecutionService = new FakeReportExecutionService();

            fakeReportExecutionService.SetupRender(catalogItemInfo, promptSelections);

            var reportViewModel = new ReportViewModel(
                catalogItemInfo,
                promptSelections,
                fakeReportExecutionService.Object,
                "ServerName");

            reportViewModel.PropertyChanged += (s, e) =>
                {
                    if(e.PropertyName == "State")
                    {
                        numberOfStateEvents++;
                    }
                    if(e.PropertyName == "Url")
                    {
                        numberOfUrlEvents++;
                    }
                };

            Assert.AreEqual(string.Empty, reportViewModel.Url);
            Assert.AreEqual(ViewModelState.Loading, reportViewModel.State);

            Assert.AreEqual(0, numberOfStateEvents);
            Assert.AreEqual(0, numberOfUrlEvents);

            fakeReportExecutionService.ExecuteRenderCallback(executionIdToCallbackWith);

            const string expectedUrl
                = "http://ServerName/Prompts.Service/ReportViewer.aspx?ExecutionId=ExecutionId";

            Assert.AreEqual(1, numberOfStateEvents);
            Assert.AreEqual(1, numberOfUrlEvents);

            Assert.AreEqual(expectedUrl, reportViewModel.Url);
            Assert.AreEqual(ViewModelState.Loaded, reportViewModel.State);
        }

        [TestMethod]
        public void ItsCorrectlyTransistionsFromUninitalizedToErrorOccuredWhenTheServiceCallsbackWithAnError()
        {
            var numberOfErrorMessageEvents = 0;
            var numberOfStateEvents = 0;
            var numberOfUrlEvents = 0;
            const string errorMessage = "ExecutionId";

            var promptSelections = A.ObservableCollection(
                A.PromptSelectionInfo().Build(),
                A.PromptSelectionInfo().Build());

            var catalogItemInfo = A.CatalogItemInfo()
                .WithName("Report Name")
                .WithPath("Report Path")
                .Build();

            var fakeReportExecutionService = new FakeReportExecutionService();

            fakeReportExecutionService.SetupRender(catalogItemInfo, promptSelections);

            var reportViewModel = new ReportViewModel(
                catalogItemInfo,
                promptSelections,
                fakeReportExecutionService.Object,
                "ServerName");

            reportViewModel.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == "State")
                    {
                        numberOfStateEvents++;
                    }
                    if (e.PropertyName == "Url")
                    {
                        numberOfUrlEvents++;
                    }
                    if(e.PropertyName == "ErrorMessage")
                    {
                        numberOfErrorMessageEvents++;
                    }
                };

            Assert.AreEqual(string.Empty, reportViewModel.Url);
            Assert.AreEqual(ViewModelState.Loading, reportViewModel.State);

            Assert.AreEqual(0, numberOfStateEvents);
            Assert.AreEqual(0, numberOfUrlEvents);
            Assert.AreEqual(0, numberOfErrorMessageEvents);

            fakeReportExecutionService.ExecuteErrorCallback(errorMessage);

            Assert.AreEqual(1, numberOfStateEvents);
            Assert.AreEqual(0, numberOfUrlEvents);
            Assert.AreEqual(1, numberOfErrorMessageEvents);

            Assert.AreEqual(errorMessage, reportViewModel.ErrorMessage);
            Assert.AreEqual(ViewModelState.Error, reportViewModel.State);
        }
    }
}
