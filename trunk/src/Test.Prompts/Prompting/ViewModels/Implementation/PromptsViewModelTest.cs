using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Prompts.Infastructure;
using Prompts.Prompting.ViewModels;
using Prompts.Prompting.ViewModels.Implementation;
using Prompts.ReportRendering.ViewModel;
using Prompts.Service.PromptService;
using Test.Prompts.Infrastructure;
using Test.Prompts.Infrastructure.Fakes;

namespace Test.Prompts.Prompting.ViewModels.Implementation
{
    [TestClass]
    public class PromptsViewModelTest
    {
        private PromptsViewModel _promptsViewModel;
        private Mock<IPopupReportViewModel> _reportsViewModel;
        private FakePromptsViewModelService _fakePromptsViewModelService;

        [TestInitialize]
        public void Setup()
        {
            _fakePromptsViewModelService = new FakePromptsViewModelService();

            _reportsViewModel = new Mock<IPopupReportViewModel>();

            _promptsViewModel = new PromptsViewModel(
                _fakePromptsViewModelService.Object
                , _reportsViewModel.Object);

        }

        [TestMethod]
        public void ItSetsTheExecuteReportCommandsCanExecuteToFalseWhenConstructed()
        {
            Assert.IsFalse(_promptsViewModel.ExecuteReport.CanExecute(null));
        }

        [TestMethod]
        public void CanExecuteIsTrueAndAnEventIsRaisedWhenAllPromptsAreReturnedFromServiceReadyForExecution()
        {
            var numberOfEvents = 0;

            var catalogItem = A.CatalogItemInfo().Build();

            _promptsViewModel.ExecuteReport.CanExecuteChanged += (s, e) => { numberOfEvents++; };

            var prompt1 = Mock.Of<IPrompt>(p => p.ReadyForReportExecution == true);
            var prompt2 = Mock.Of<IPrompt>(p => p.ReadyForReportExecution == true);
            var prompt3 = Mock.Of<IPrompt>(p => p.ReadyForReportExecution == true);
            var promptsToCallbackWith = A.Array(prompt1, prompt2, prompt3);

            _fakePromptsViewModelService.SetupGetPrompts(catalogItem.Path);

            _promptsViewModel.ShowPromptsFor(catalogItem);

            _fakePromptsViewModelService.ExecuteCallback(promptsToCallbackWith);

            Assert.AreEqual(1, numberOfEvents);
            Assert.IsTrue(_promptsViewModel.ExecuteReport.CanExecute(null));
        }

        [TestMethod]
        public void CanExecuteStaysFalseAndNoEventsAreRaisedWhenOneOfThePromptsChangesToReadyButAllPromptsAreStillNotReady()
        {
            var numberOfEvents = 0;

            var catalogItem = A.CatalogItemInfo().WithPath("Report 1").Build();

            var prompt1 = Mock.Of<IPrompt>(p => p.ReadyForReportExecution == true);
            var prompt2 = new Mock<IPrompt>();
            prompt2.SetupGet(p => p.ReadyForReportExecution).Returns(false);
            var prompt3 = Mock.Of<IPrompt>(p => p.ReadyForReportExecution == false);

            _fakePromptsViewModelService.SetupGetPrompts(catalogItem.Path);

            _promptsViewModel.ShowPromptsFor(catalogItem);

            _fakePromptsViewModelService.ExecuteCallback(A.ObservableCollection(prompt1, prompt2.Object, prompt3));

            Assert.IsFalse(_promptsViewModel.ExecuteReport.CanExecute(null));

            _promptsViewModel.ExecuteReport.CanExecuteChanged += (s, e) => { numberOfEvents++; };

            prompt2.SetupGet(p => p.ReadyForReportExecution).Returns(true);
            prompt2.Raise(p => p.PropertyChanged += null, new PropertyChangedEventArgs("ReadyForReportExecution"));

            Assert.AreEqual(numberOfEvents, 0);
            Assert.IsFalse(_promptsViewModel.ExecuteReport.CanExecute(null));
        }

        [TestMethod]
        public void CanExecuteReportIsTrueAndAnEventIsRaisedWhenTheFinalPromptChangesToReady()
        {
            var numberOfEvents = 0;

            var catalogItem = A.CatalogItemInfo().WithPath("Report 1").Build();

            _fakePromptsViewModelService.SetupGetPrompts(catalogItem.Path);

            var prompt1 = Mock.Of<IPrompt>(p => p.ReadyForReportExecution == true);
            var prompt2 = Mock.Of<IPrompt>(p => p.ReadyForReportExecution == true);
            var prompt3 = new Mock<IPrompt>();
            prompt3.SetupGet(p => p.ReadyForReportExecution).Returns(false);

            _promptsViewModel.ShowPromptsFor(catalogItem);

            _fakePromptsViewModelService.ExecuteCallback(A.ObservableCollection(prompt1, prompt2, prompt3.Object));

            Assert.IsFalse(_promptsViewModel.ExecuteReport.CanExecute(null));

            _promptsViewModel.ExecuteReport.CanExecuteChanged += (s, e) => { numberOfEvents++; };

            prompt3.SetupGet(p => p.ReadyForReportExecution).Returns(true);
            prompt3.Raise(p => p.PropertyChanged += null, new PropertyChangedEventArgs("ReadyForReportExecution"));

            Assert.AreEqual(numberOfEvents, 1);
            Assert.IsTrue(_promptsViewModel.ExecuteReport.CanExecute(null));
        }

        [TestMethod]
        public void CanExecuteChangesToFalseAndAnEventIsFiredWhenOneOfThePromtpsBecomesNotReady()
        {
            var numberOfEvents = 0;

            var catalogItem = A.CatalogItemInfo().WithPath("Report 1").Build();

            var prompt1 = Mock.Of<IPrompt>(p => p.ReadyForReportExecution == true);
            var prompt2 = Mock.Of<IPrompt>(p => p.ReadyForReportExecution == true);
            var prompt3 = new Mock<IPrompt>();
            prompt3.SetupGet(p => p.ReadyForReportExecution).Returns(false);

            _fakePromptsViewModelService.SetupGetPrompts(catalogItem.Path);

            _promptsViewModel.ShowPromptsFor(catalogItem);

            _fakePromptsViewModelService.ExecuteCallback(A.ObservableCollection(prompt1, prompt2, prompt3.Object));

            Assert.IsFalse(_promptsViewModel.ExecuteReport.CanExecute(null));

            _promptsViewModel.ExecuteReport.CanExecuteChanged += (s, e) => { numberOfEvents++; };

            prompt3.SetupGet(p => p.ReadyForReportExecution).Returns(true);
            prompt3.Raise(p => p.PropertyChanged += null, new PropertyChangedEventArgs("ReadyForReportExecution"));

            Assert.AreEqual(numberOfEvents, 1);
            Assert.IsTrue(_promptsViewModel.ExecuteReport.CanExecute(null));
        }

        [TestMethod]
        public void ChangesFromUnInitializedToLoadingToLoadedWhenPromptsLoadSucessfully()
        {
            var numberOfStateEvents = 0;
            var numberOfPromptsEvents = 0;
            var numberOfReportNameEvents = 0;

            var catalogItem = A.CatalogItemInfo().WithPath("Report 1").Build();

            var promptsToCallBackWith = A.ObservableCollection(A.Prompt().Build(), A.Prompt().Build());

            _promptsViewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName.Equals("State"))
                {
                    numberOfStateEvents++;
                }
                if (e.PropertyName.Equals("ReportName"))
                {
                    numberOfReportNameEvents++;
                }
                if (e.PropertyName.Equals("Prompts"))
                {
                    numberOfPromptsEvents++;
                }
            };

            Assert.AreEqual(ViewModelState.UnInitialized, _promptsViewModel.State);
            Assert.AreEqual(0, numberOfStateEvents);
            Assert.AreEqual(string.Empty, _promptsViewModel.ReportName);
            Assert.AreEqual(0, numberOfReportNameEvents);
            Assert.AreEqual(0, numberOfPromptsEvents);
            _promptsViewModel.Prompts.AssertLength(0);

            _fakePromptsViewModelService.SetupGetPrompts(catalogItem.Path);

            _promptsViewModel.ShowPromptsFor(catalogItem);

            Assert.AreEqual(ViewModelState.Loading, _promptsViewModel.State);
            Assert.AreEqual(1, numberOfStateEvents);
            Assert.AreEqual(catalogItem.Name, _promptsViewModel.ReportName);
            Assert.AreEqual(1, numberOfReportNameEvents);
            _promptsViewModel.Prompts.AssertLength(0);
            Assert.AreEqual(0, numberOfPromptsEvents);

            _fakePromptsViewModelService.ExecuteCallback(promptsToCallBackWith);

            Assert.AreEqual(ViewModelState.Loaded, _promptsViewModel.State);
            Assert.AreEqual(2, numberOfStateEvents);
            Assert.AreEqual(catalogItem.Name, _promptsViewModel.ReportName);
            Assert.AreEqual(1, numberOfReportNameEvents);
            _promptsViewModel.Prompts.AssertEquals(promptsToCallBackWith);
            Assert.AreEqual(1, numberOfPromptsEvents);
        }

        [TestMethod]
        public void StateChangesFromUnInitializedToLoadingToErrorWhenAnErrorOccursGettingPrompts()
        {
            var numberOfStateEvents = 0;
            var numberOfErrorMessageEvents = 0;

            var catalogItem = A.CatalogItemInfo().Build();

            _promptsViewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName.Equals("State"))
                {
                    numberOfStateEvents++;
                }
                if (e.PropertyName.Equals("ErrorMessage"))
                {
                    numberOfErrorMessageEvents++;
                }
            };

            Assert.AreEqual(ViewModelState.UnInitialized, _promptsViewModel.State);
            Assert.AreEqual(0, numberOfStateEvents);

            _fakePromptsViewModelService.SetupGetPrompts(catalogItem.Path);
            _promptsViewModel.ShowPromptsFor(catalogItem);
            Assert.AreEqual(ViewModelState.Loading, _promptsViewModel.State);
            Assert.AreEqual(1, numberOfStateEvents);

            const string errorMessage = "Error Message";
            Assert.AreEqual(0, numberOfErrorMessageEvents);

            _fakePromptsViewModelService.ExecuteErrorCallback(errorMessage);

            Assert.AreEqual(ViewModelState.Error, _promptsViewModel.State);
            Assert.AreEqual(2, numberOfStateEvents);

            Assert.AreEqual(1, numberOfErrorMessageEvents);
            Assert.AreEqual(errorMessage, _promptsViewModel.ErrorMessage);

            _promptsViewModel.ShowPromptsFor(catalogItem);
            Assert.AreEqual(3, numberOfStateEvents);
            Assert.AreEqual(2, numberOfErrorMessageEvents);
            Assert.AreEqual(ViewModelState.Loading, _promptsViewModel.State);
            Assert.AreEqual(String.Empty, _promptsViewModel.ErrorMessage);
        }

        [TestMethod]
        public void RenderReport()
        {
            var catalogItem = A.CatalogItemInfo().Build();

            var prompt1SelectionInfo = A.PromptSelectionInfo().WithPromptName("Prompt 1").Build();
            var prompt2SelectionInfo = A.PromptSelectionInfo().WithPromptName("Prompt 2").Build();

            var prompt1 = Mock.Of<IPrompt>(p => p.ToSelectionInfo() == prompt1SelectionInfo);
            var prompt2 = Mock.Of<IPrompt>(p => p.ToSelectionInfo() == prompt2SelectionInfo);

            _fakePromptsViewModelService.SetupGetPrompts(catalogItem.Path);

            _promptsViewModel.ShowPromptsFor(catalogItem);

            _fakePromptsViewModelService.ExecuteCallback(A.Array(prompt1, prompt2));

            _promptsViewModel.ExecuteReport.Execute(null);

            Predicate<ObservableCollection<PromptSelectionInfo>> predicate = c =>
                { 
                    c.AssertSingle(i => i.Equals(prompt2SelectionInfo));
                    c.AssertSingle(i => i.Equals(prompt1SelectionInfo));
                    return true;
                };

            _reportsViewModel.Verify(vm => vm.AddReport(catalogItem, Match.Create(predicate)), Times.Exactly(1));
        }

        [TestMethod]
        public void ItSelectsTheNextPromptsWhenTheMoveNextPromptCommandIsExecuted()
        {
            var numberOfSelectedPromptEvents = 0;

            var catalogItem = A.CatalogItemInfo().Build();

            _promptsViewModel.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == "SelectedPrompt")
                    {
                        numberOfSelectedPromptEvents++;
                    }
                };

            _fakePromptsViewModelService.SetupGetPrompts(catalogItem.Path);

            _promptsViewModel.ShowPromptsFor(catalogItem);

            var prompt1 = new Mock<IPrompt>().Object;
            var prompt2 = new Mock<IPrompt>().Object;
            var promptsToCallBackWith = new[] {prompt1, prompt2};

            _fakePromptsViewModelService.ExecuteCallback(promptsToCallBackWith);

            Assert.AreEqual(1, numberOfSelectedPromptEvents);
            Assert.AreEqual(prompt1, _promptsViewModel.SelectedPrompt);

            _promptsViewModel.MoveNext.Execute(null);
            Assert.AreEqual(2, numberOfSelectedPromptEvents);
            Assert.AreEqual(prompt2, _promptsViewModel.SelectedPrompt);
        }

        [TestMethod]
        public void ItSelectsThePreviousPromptsWhenTheMoveNextPromptCommandIsExecuted()
        {
            var numberOfSelectedPromptEvents = 0;

            var catalogItem = A.CatalogItemInfo().Build();

            _fakePromptsViewModelService.SetupGetPrompts(catalogItem.Path);

            _promptsViewModel.ShowPromptsFor(catalogItem);

            var prompt1 = new Mock<IPrompt>().Object;
            var prompt2 = new Mock<IPrompt>().Object;
            var promptsToCallBackWith = new[] { prompt1, prompt2 };

            _fakePromptsViewModelService.ExecuteCallback(promptsToCallBackWith);

            _promptsViewModel.MoveNext.Execute(null);
            Assert.AreEqual(prompt2, _promptsViewModel.SelectedPrompt);

            _promptsViewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "SelectedPrompt")
                {
                    numberOfSelectedPromptEvents++;
                }
            };

            _promptsViewModel.MovePrevious.Execute(null);

            Assert.AreEqual(1, numberOfSelectedPromptEvents);
            Assert.AreEqual(prompt1, _promptsViewModel.SelectedPrompt);
        }

        [TestMethod]
        public void ItSetsTheMovePreviousToFalseWhenThePromptsAreInitallyLoaded()
        {
            var catalogItem = A.CatalogItemInfo().Build();

            _fakePromptsViewModelService.SetupGetPrompts(catalogItem.Path);

            _promptsViewModel.ShowPromptsFor(catalogItem);

            var prompt1 = Mock.Of<IPrompt>();
            var prompt2 = Mock.Of<IPrompt>();
            var promptsToCallBackWith = A.Array(prompt1, prompt2);

            _fakePromptsViewModelService.ExecuteCallback(promptsToCallBackWith);

            Assert.IsFalse(_promptsViewModel.MovePrevious.CanExecute(null));
        }

        [TestMethod]
        public void ItSetsTheMovePreviousToTrueWhenTheSelectedItemChangesFromTheFirstToTheSecond()
        {
            var numberOfCanExecuteEvents = 0;

            var catalogItem = A.CatalogItemInfo().Build();

            _fakePromptsViewModelService.SetupGetPrompts(catalogItem.Path);

            _promptsViewModel.ShowPromptsFor(catalogItem);

            var prompt1 = Mock.Of<IPrompt>();
            var prompt2 = Mock.Of<IPrompt>();
            var promptsToCallBackWith = A.Array(prompt1, prompt2);

            _fakePromptsViewModelService.ExecuteCallback(promptsToCallBackWith);

            _promptsViewModel.MovePrevious.CanExecuteChanged += (s, e) =>
                {
                    numberOfCanExecuteEvents++;
                };

            _promptsViewModel.MoveNext.Execute(null);
            Assert.AreEqual(1, numberOfCanExecuteEvents);
            Assert.IsTrue(_promptsViewModel.MovePrevious.CanExecute(null));
        }

        [TestMethod]
        public void ItSetsTheMovePreviousToFalseWhenTheSelectedItemChangesFromTheSecondToFirst()
        {
            var numberOfCanExecuteEvents = 0;

            var catalogItem = A.CatalogItemInfo().Build();

            _fakePromptsViewModelService.SetupGetPrompts(catalogItem.Path);

            _promptsViewModel.ShowPromptsFor(catalogItem);

            var prompt1 = Mock.Of<IPrompt>();
            var prompt2 = Mock.Of<IPrompt>();
            var promptsToCallBackWith = A.Array(prompt1, prompt2);

            _fakePromptsViewModelService.ExecuteCallback(promptsToCallBackWith);

            _promptsViewModel.MoveNext.Execute(null);

            _promptsViewModel.MovePrevious.CanExecuteChanged += (s, e) =>
            {
                numberOfCanExecuteEvents++;
            };

            _promptsViewModel.MovePrevious.Execute(null);
            Assert.AreEqual(1, numberOfCanExecuteEvents);
            Assert.IsFalse(_promptsViewModel.MovePrevious.CanExecute(null));
        }

        [TestMethod]
        public void ItSetsTheMovePreviousToFalseWhenTheFirstPromptIsSelected()
        {
            var numberOfCanExecuteEvents = 0;

            var catalogItem = A.CatalogItemInfo().Build();

            _fakePromptsViewModelService.SetupGetPrompts(catalogItem.Path);

            _promptsViewModel.ShowPromptsFor(catalogItem);

            var prompt1 = new Mock<IPrompt>().Object;
            var prompt2 = new Mock<IPrompt>().Object;
            var promptsToCallBackWith = new[] { prompt1, prompt2 };

            _fakePromptsViewModelService.ExecuteCallback(promptsToCallBackWith);

            _promptsViewModel.SelectedPrompt = prompt2;

            _promptsViewModel.MovePrevious.CanExecuteChanged += (s, e) =>
            {
                numberOfCanExecuteEvents++;
            };

            _promptsViewModel.SelectedPrompt = prompt1;
            Assert.AreEqual(1, numberOfCanExecuteEvents);
            Assert.IsFalse(_promptsViewModel.MovePrevious.CanExecute(null));
        }

        [TestMethod]
        public void ItSetsTheMovePreviousToTrueWhenSelectedPromptChangesFromTheFirstToAnother()
        {
            var numberOfCanExecuteEvents = 0;

            var catalogItem = A.CatalogItemInfo().Build();

            _fakePromptsViewModelService.SetupGetPrompts(catalogItem.Path);

            _promptsViewModel.ShowPromptsFor(catalogItem);

            var prompt1 = Mock.Of<IPrompt>();
            var prompt2 = Mock.Of<IPrompt>();
            var promptsToCallBackWith = A.Array(prompt1, prompt2);

            _fakePromptsViewModelService.ExecuteCallback(promptsToCallBackWith);

            _promptsViewModel.SelectedPrompt = prompt1;

            _promptsViewModel.MovePrevious.CanExecuteChanged += (s, e) =>
            {
                numberOfCanExecuteEvents++;
            };

            _promptsViewModel.SelectedPrompt = prompt2;
            Assert.AreEqual(1, numberOfCanExecuteEvents);
            Assert.IsTrue(_promptsViewModel.MovePrevious.CanExecute(null));
        }

        [TestMethod]
        public void ItSetsTheCanMoveNextToTrueWhenThePromptLoadsAndThereAreMoreThanOnePrompts()
        {
            var catalogItem = A.CatalogItemInfo().Build();

            _fakePromptsViewModelService.SetupGetPrompts(catalogItem.Path);

            _promptsViewModel.ShowPromptsFor(catalogItem);

            var prompt1 = Mock.Of<IPrompt>();
            var prompt2 = Mock.Of<IPrompt>();
            var promptsToCallBackWith = A.Array(prompt1, prompt2);

            _fakePromptsViewModelService.ExecuteCallback(promptsToCallBackWith);

            Assert.IsTrue(_promptsViewModel.MoveNext.CanExecute(null));
        }

        [TestMethod]
        public void ItSetsTheCanMoveNextToFalseWhenTheSelectedPromptChangesToTheLastPrompt()
        {
            var numberOfCanExecuteEvents = 0;

            var catalogItem = A.CatalogItemInfo().Build();

            _fakePromptsViewModelService.SetupGetPrompts(catalogItem.Path);

            _promptsViewModel.ShowPromptsFor(catalogItem);

            var prompt1 = Mock.Of<IPrompt>();
            var prompt2 = Mock.Of<IPrompt>();
            var promptsToCallBackWith = A.Array(prompt1, prompt2);

            _fakePromptsViewModelService.ExecuteCallback(promptsToCallBackWith);

            _promptsViewModel.MoveNext.CanExecuteChanged += (s, e) =>
                {
                    numberOfCanExecuteEvents++;
                };

            _promptsViewModel.MoveNext.Execute(null);

            Assert.AreEqual(1, numberOfCanExecuteEvents);
            Assert.IsFalse(_promptsViewModel.MoveNext.CanExecute(null));
        }

        [TestMethod]
        public void ItSetsTheCanMoveNextToFalseWhenTheLastPromptIsSelected()
        {
            var numberOfCanExecuteEvents = 0;

            var catalogItem = A.CatalogItemInfo().Build();

            _fakePromptsViewModelService.SetupGetPrompts(catalogItem.Path);

            _promptsViewModel.ShowPromptsFor(catalogItem);

            var prompt1 = Mock.Of<IPrompt>();
            var prompt2 = Mock.Of<IPrompt>();
            var promptsToCallBackWith = A.Array(prompt1, prompt2);

            _fakePromptsViewModelService.ExecuteCallback(promptsToCallBackWith);

            _promptsViewModel.MoveNext.CanExecuteChanged += (s, e) =>
            {
                numberOfCanExecuteEvents++;
            };

            _promptsViewModel.SelectedPrompt = prompt2;

            Assert.AreEqual(1, numberOfCanExecuteEvents);
            Assert.IsFalse(_promptsViewModel.MoveNext.CanExecute(null));
        }

        [TestMethod]
        public void ItSetsTheCanMoveNextToTrueWhenTheSelectedPromptChangesFromTheLastToAnother()
        {
            var numberOfCanExecuteEvents = 0;

            var catalogItem = A.CatalogItemInfo().Build();

            _fakePromptsViewModelService.SetupGetPrompts(catalogItem.Path);

            _promptsViewModel.ShowPromptsFor(catalogItem);

            var prompt1 = Mock.Of<IPrompt>();
            var prompt2 = Mock.Of<IPrompt>();
            var promptsToCallBackWith = A.Array(prompt1, prompt2);

            _fakePromptsViewModelService.ExecuteCallback(promptsToCallBackWith);

            _promptsViewModel.SelectedPrompt = prompt2;

            _promptsViewModel.MoveNext.CanExecuteChanged += (s, e) =>
            {
                numberOfCanExecuteEvents++;
            };

            _promptsViewModel.SelectedPrompt = prompt1;

            Assert.AreEqual(1, numberOfCanExecuteEvents);
            Assert.IsTrue(_promptsViewModel.MoveNext.CanExecute(null));
        }

        [TestMethod]
        public void ItSetsTheCanMoveNextToTrueWhenTheSelectedPromptChangesFromTheLastPromptToThePrevious()
        {
            var numberOfCanExecuteEvents = 0;

            var catalogItem = A.CatalogItemInfo().Build();

            _fakePromptsViewModelService.SetupGetPrompts(catalogItem.Path);

            _promptsViewModel.ShowPromptsFor(catalogItem);

            var prompt1 = Mock.Of<IPrompt>();
            var prompt2 = Mock.Of<IPrompt>();
            var promptsToCallBackWith = A.Array(prompt1, prompt2);

            _fakePromptsViewModelService.ExecuteCallback(promptsToCallBackWith);

            _promptsViewModel.MoveNext.Execute(null);

            _promptsViewModel.MoveNext.CanExecuteChanged += (s, e) =>
            {
                numberOfCanExecuteEvents++;
            };

            _promptsViewModel.MovePrevious.Execute(null);

            Assert.AreEqual(1, numberOfCanExecuteEvents);
            Assert.IsTrue(_promptsViewModel.MoveNext.CanExecute(null));
        }

        [TestMethod]
        public void ItSetsTheCanMoveNextToFalseWhenThePromptLoadsAndThereIsOnlyOnePrompt()
        {
            var catalogItem = A.CatalogItemInfo().Build();

            _fakePromptsViewModelService.SetupGetPrompts(catalogItem.Path);

            _promptsViewModel.ShowPromptsFor(catalogItem);

            var prompt1 = Mock.Of<IPrompt>();
            var promptsToCallBackWith = A.Array(prompt1);

            _fakePromptsViewModelService.ExecuteCallback(promptsToCallBackWith);

            Assert.IsFalse(_promptsViewModel.MoveNext.CanExecute(null));
        }

        [TestMethod]
        public void ItDoesNotResendTheRequestIfItIsForTheSameReport()
        {
            var catalogItemInfo = A.CatalogItemInfo().WithName("Report 1").Build();
            var promptsToCallbackWith = A.ObservableCollection(A.Prompt().Build(), A.Prompt().Build());

            _fakePromptsViewModelService.SetupGetPrompts(catalogItemInfo.Path);

            _promptsViewModel.ShowPromptsFor(catalogItemInfo);

            _promptsViewModel.ShowPromptsFor(catalogItemInfo);

            _fakePromptsViewModelService.ExecuteCallback(promptsToCallbackWith);

            _fakePromptsViewModelService.AssertNumberOfGetPrompts(catalogItemInfo.Path, Times.Exactly(1));
        }

        [TestMethod]
        public void ItCancelsTheRequestToRenderAReportIfANewRequestIsMadeBeforeTheFirstReturns()
        {
            var catalogItemInfo1 = A.CatalogItemInfo().WithPath("Report 1").Build();
            var catalogItemInfo2 = A.CatalogItemInfo().WithPath("Report 2").Build();

            _fakePromptsViewModelService.SetupGetPrompts(catalogItemInfo1.Path);

            _promptsViewModel.ShowPromptsFor(catalogItemInfo1);

            _fakePromptsViewModelService.AssertNumberOfGetPrompts(catalogItemInfo1.Path, Times.Exactly(1));

            _promptsViewModel.ShowPromptsFor(catalogItemInfo2);

            _fakePromptsViewModelService.AssertNumberOfCancels(catalogItemInfo1.Path, Times.Exactly(1));
            _fakePromptsViewModelService.AssertNumberOfGetPrompts(catalogItemInfo2.Path, Times.Exactly(1));
        }
    }
}