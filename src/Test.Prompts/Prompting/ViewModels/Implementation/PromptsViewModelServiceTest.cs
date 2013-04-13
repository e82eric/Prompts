using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Prompts.Prompting.Construction;
using Prompts.Prompting.ViewModels.Implementation;
using Prompts.PromptServiceProxy;
using Prompts.Service.PromptService;
using Test.Prompts.Infrastructure;
using Test.Prompts.Infrastructure.Fakes;

namespace Test.Prompts.Prompting.ViewModels.Implementation
{
    [TestClass]
    public class PromptsViewModelServiceTest
    {
        private Mock<IPromptsViewModelBuilder> _promptCollectionBuilder;
        private PromptsViewModelService _promptsViewModelService;
        private FakePromptServiceClient _fakePromptServiceClient;

        [TestInitialize]
        public void Setup()
        {
            var promptServiceClient = new Mock<IPromptServiceClient>();
            _fakePromptServiceClient = new FakePromptServiceClient(promptServiceClient);
            _promptCollectionBuilder = new Mock<IPromptsViewModelBuilder>();
            _promptsViewModelService = new PromptsViewModelService(
                _promptCollectionBuilder.Object,
                promptServiceClient.Object);
        }

        [TestMethod]
        public void CallsbackWithPromptsBuiltByBuilder()
        {
            var numberOfCallBacks = 0;

            const string reportPath = "Report Path";
            var promptsFromClientService = A.ObservableCollection(A.PromptInfo().Build(), A.PromptInfo().Build());
            var promptsFromBuilder = A.ObservableCollection(A.Prompt().Build(), A.Prompt().Build());

            _promptCollectionBuilder.Setup(
                b => b.BuildFrom(
                    reportPath,
                    promptsFromClientService)
                ).Returns(
                    promptsFromBuilder);

            _fakePromptServiceClient.SetupGetPromptsAsync(reportPath);

            _promptsViewModelService.GetPromptViewModels(
                reportPath
                , c =>
                    {
                        c.AssertEquals(promptsFromBuilder);
                        numberOfCallBacks++;
                    },
                e => { });

            Assert.AreEqual(0, numberOfCallBacks);

            _fakePromptServiceClient.RaiseGetPromptsCompleted(promptsFromClientService, reportPath);

            Assert.AreEqual(1, numberOfCallBacks);
        }

        [TestMethod]
        public void ItUsesTheCorrectCallbackBasedOnTheReportPath()
        {
            var numberOfReport1Callbacks = 0;
            var numberOfReport2Callbacks = 0;

            const string reportPath1 = "Path 1";
            const string reportPath2 = "Path 2";

            var report1PromptInfos = A.ObservableCollection(A.PromptInfo().Build());
            var report2PromptInfos = A.ObservableCollection(A.PromptInfo().Build(), A.PromptInfo().Build());
            
            var report1Prompts = A.ObservableCollection(A.Prompt().Build());
            var report2Prompts = A.ObservableCollection(A.Prompt().Build(), A.Prompt().Build());

            _promptCollectionBuilder
                .Setup(b => b.BuildFrom(reportPath1, report1PromptInfos))
                .Returns(report1Prompts);

            _promptCollectionBuilder
                .Setup(b => b.BuildFrom(reportPath2, report2PromptInfos))
                .Returns(report2Prompts);

            _fakePromptServiceClient.SetupGetPromptsAsync(reportPath1);
            _fakePromptServiceClient.SetupGetPromptsAsync(reportPath2);

            _promptsViewModelService.GetPromptViewModels(
                reportPath1, 
                c =>
                {
                    c.AssertEquals(report1Prompts);
                    numberOfReport1Callbacks++;
                },
                e =>{});

            _promptsViewModelService.GetPromptViewModels(
                reportPath2,
                c =>
                    {
                        c.AssertEquals(report2Prompts);
                        numberOfReport2Callbacks++;
                    },
                e => { });

            Assert.AreEqual(0, numberOfReport1Callbacks);
            Assert.AreEqual(0, numberOfReport2Callbacks);

            _fakePromptServiceClient.RaiseGetPromptsCompleted(report2PromptInfos, reportPath2);

            Assert.AreEqual(0, numberOfReport1Callbacks);
            Assert.AreEqual(1, numberOfReport2Callbacks);

            _fakePromptServiceClient.RaiseGetPromptsCompleted(report1PromptInfos, reportPath1);

            Assert.AreEqual(1, numberOfReport1Callbacks);
            Assert.AreEqual(1, numberOfReport2Callbacks);
        }

        [TestMethod]
        public void ItCanGetPromptsForTheSameReportMoreThanOnce()
        {
            var numberOfCallbacks1 = 0;
            var numberOfCallbacks2 = 0;

            const string reportPath1 = "Path 1";

            var report1PromptInfos = A.ObservableCollection(A.PromptInfo().Build());

            var report1Prompts = A.ObservableCollection(A.Prompt().Build());

            _promptCollectionBuilder
                .Setup(b => b.BuildFrom(reportPath1, report1PromptInfos))
                .Returns(report1Prompts);

            _fakePromptServiceClient.SetupGetPromptsAsync(reportPath1);

            _promptsViewModelService.GetPromptViewModels(
                reportPath1,
                c =>
                    {
                        c.AssertEquals(report1Prompts);
                        numberOfCallbacks1++;
                    },
                m => { });

            Assert.AreEqual(0, numberOfCallbacks1);

            _fakePromptServiceClient.RaiseGetPromptsCompleted(report1PromptInfos, reportPath1);

            Assert.AreEqual(0, numberOfCallbacks2);
            Assert.AreEqual(1, numberOfCallbacks1);

            _promptsViewModelService.GetPromptViewModels(
                reportPath1,
                c =>
                    {
                        c.AssertEquals(report1Prompts);
                        numberOfCallbacks2++;
                    },
                m => { });

            _fakePromptServiceClient.RaiseGetPromptsCompleted(report1PromptInfos, reportPath1);

            Assert.AreEqual(1, numberOfCallbacks2);
            Assert.AreEqual(1, numberOfCallbacks1);
        }

        [TestMethod]
        public void RaisesEventWhenThePromptCollectionIndicatesThatAnErrorOccured()
        {
            var numberOfCallBacks = 0;
            var numberOfErrorEvents = 0;

            const string reportPath = "Report Name";
            const string errorMessage = "Error Message";

            var promptsFromClientService = A.ObservableCollection(A.PromptInfo().Build(), A.PromptInfo().Build());
            var promptsFromBuilder = A.ObservableCollection(A.Prompt().Build(), A.Prompt().Build());

            _promptCollectionBuilder
                .Setup(b => b.BuildFrom(reportPath, promptsFromClientService))
                .Returns(promptsFromBuilder);

            _fakePromptServiceClient.SetupGetPromptsAsync(reportPath);

            _promptsViewModelService.GetPromptViewModels(
                reportPath, 
                c => { numberOfCallBacks++; },
                m =>
                    {
                        numberOfErrorEvents++;
                        Assert.AreEqual(errorMessage, m);
                    });

            Assert.AreEqual(0, numberOfCallBacks);

            _fakePromptServiceClient.RaiseGetPromptsCompletedWithError(errorMessage, reportPath);

            Assert.AreEqual(0, numberOfCallBacks);
            Assert.AreEqual(1, numberOfErrorEvents);
        }

        [TestMethod]
        public void ItCanGetThePromptsForAReportAfterAnErrorHasOccuredForThatRepor()
        {
            var numberOfCallBacks = 0;
            var numberOfErrorEvents = 0;

            const string reportPath = "Report Name";
            const string errorMessage = "Error Message";

            var promptsFromClientService = A.ObservableCollection(A.PromptInfo().Build(), A.PromptInfo().Build());
            var promptsFromBuilder = A.ObservableCollection(A.Prompt().Build(), A.Prompt().Build());

            _promptCollectionBuilder
                .Setup(b => b.BuildFrom(reportPath, promptsFromClientService))
                .Returns(promptsFromBuilder);

            _fakePromptServiceClient.SetupGetPromptsAsync(reportPath);

            _promptsViewModelService.GetPromptViewModels(
                reportPath, 
                c => { numberOfCallBacks++; },
                m =>
                    {
                        numberOfErrorEvents++;
                        Assert.AreEqual(errorMessage, m);
                    });

            Assert.AreEqual(0, numberOfCallBacks);

            _fakePromptServiceClient.RaiseGetPromptsCompletedWithError(errorMessage, reportPath);

            Assert.AreEqual(0, numberOfCallBacks);
            Assert.AreEqual(1, numberOfErrorEvents);

            _promptsViewModelService.GetPromptViewModels(
                reportPath,
                c =>
                    {
                        numberOfCallBacks++;
                    },
                m =>
                    {
                        numberOfErrorEvents++;
                        Assert.AreEqual(errorMessage, m);
                    });

            Assert.AreEqual(0, numberOfCallBacks);

            _fakePromptServiceClient.RaiseGetPromptsCompleted(new PromptInfo[]{}, reportPath);

            Assert.AreEqual(1, numberOfCallBacks);
            Assert.AreEqual(1, numberOfErrorEvents);
        }

        [TestMethod]
        public void ItDoesNotCallbackWhenCanceled()
        {
            var numberOfCallbacks = 0;

            const string reportPath = "Report Name";

            var promptInfos = A.Array(A.PromptInfo().Build());

            _fakePromptServiceClient.SetupGetPromptsAsync(reportPath);

            _promptsViewModelService.GetPromptViewModels(
                reportPath, 
                c => { numberOfCallbacks++; },
                m => {});

            _promptsViewModelService.CancelGetPromptViewModels(reportPath);

            _fakePromptServiceClient.RaiseGetPromptsCompleted(promptInfos, reportPath);

            Assert.AreEqual(0, numberOfCallbacks);
        }
    }
}