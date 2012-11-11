using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Prompts.Infastructure;
using Prompts.Prompting.ViewModels;
using Prompts.Prompting.ViewModels.Implementation;
using Test.Prompts.Infrastructure;
using Test.Prompts.Infrastructure.Fakes;

namespace Test.Prompts.Prompting.ViewModels.Implementation
{
    [TestClass]
    public class MultiSelectCasscadingSearchTest
    {
        private MultiSelectCasscadingSearch _viewModel;
        private FakeAsynchronousSearchService _searchService;

        [TestInitialize]
        public void Setup()
        {
            _searchService = new FakeAsynchronousSearchService();
            _viewModel = new MultiSelectCasscadingSearch(
                "label"
                , "name"
                , _searchService.Object
                , new ObservableCollection<ISearchablePromptItem>()
                , new ObservableCollection<ISearchablePromptItem>());
        }

        [TestMethod]
        public void ItSetsTheAvailableItemsToTheDefaultSelections()
        {
            var availableItems = A.ObservableCollection(Mock.Of<ISearchablePromptItem>());

            var viewModel = new MultiSelectCasscadingSearch(
                "label"
                , "name"
                , _searchService.Object
                , availableItems
                , new ObservableCollection<ISearchablePromptItem>());

            Assert.AreEqual(availableItems, viewModel.AvailableItems);
        }

        [TestMethod]
        public void ItSetsTheStateToLoadedWhenThereIsAAvailableItem()
        {
            var availableItems = A.ObservableCollection(Mock.Of<ISearchablePromptItem>());

            var viewModel = new MultiSelectCasscadingSearch(
                "label"
                , "name"
                , _searchService.Object
                , availableItems
                , new ObservableCollection<ISearchablePromptItem>());

            Assert.AreEqual(ViewModelState.Loaded, viewModel.State);
        }

        [TestMethod]
        public void ItSetsTheStateToLoadedWhenThereIsNotAAvailableItem()
        {
            var viewModel = new MultiSelectCasscadingSearch(
                "label"
                , "name"
                , _searchService.Object
                , new ObservableCollection<ISearchablePromptItem>()
                , new ObservableCollection<ISearchablePromptItem>());

            Assert.AreEqual(ViewModelState.UnInitialized, viewModel.State);
        }

        [TestMethod]
        public void IsSetsTheAvailableItemsToTheResultOfSearchServiceWhenASearchIsExecuted()
        {
            var numberOfAvailableItemsEvents = 0;
            const string searchExpresson = "Search Expression";
            _searchService.SetupSearch(searchExpresson);
            var promptItemsToCallbackWith = A.ObservableCollection(Mock.Of<ISearchablePromptItem>());

            _viewModel.PropertyChanged += (s, e) =>
                {
                    if(e.PropertyName == "AvailableItems")
                    {
                        numberOfAvailableItemsEvents++;
                    }
                };

            _viewModel.SearchString = searchExpresson;

            _viewModel.Search.Execute(null);

            Assert.AreEqual(0, numberOfAvailableItemsEvents);

            _searchService.ExecuteSearchCallback(promptItemsToCallbackWith);

            Assert.AreEqual(1, numberOfAvailableItemsEvents);
            Assert.AreEqual(promptItemsToCallbackWith, _viewModel.AvailableItems);
        }

        [TestMethod]
        public void ItSetsTheStateAndErrorMessageWhenTheChildPromptItemServiceRaisesAnError()
        {
            const string errorMessage = "Error Message";

            var numberOfAvailableItemsEvents = 0;
            var numberOfStateEvents = 0;
            var numberOfErrorMessageEvents = 0;
            const string searchExpresson = "Search Expression";
            _searchService.SetupSearch(searchExpresson);

            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "AvailableItems")
                {
                    numberOfAvailableItemsEvents++;
                }
                if(e.PropertyName == "State")
                {
                    numberOfStateEvents++;
                }
                if (e.PropertyName == "ErrorMessage")
                {
                    numberOfErrorMessageEvents++;
                }
            };

            _viewModel.SearchString = searchExpresson;

            _viewModel.Search.Execute(null);

            Assert.AreEqual(0, numberOfAvailableItemsEvents);

            _searchService.ExecuteErrorCallback(errorMessage);

            Assert.AreEqual(1, numberOfAvailableItemsEvents);
            _viewModel.AvailableItems.AssertLength(0);
            Assert.AreEqual(2, numberOfStateEvents);
            Assert.AreEqual(ViewModelState.Error, _viewModel.State);
            Assert.AreEqual(errorMessage, _viewModel.ErrorMessage);
            Assert.AreEqual(1, numberOfErrorMessageEvents);
        }

        [TestMethod]
        public void ItSetsTheStateToLoadingWhenASearchIsSubmitted()
        {
            var numberOfStateEvents = 0;

            _viewModel.PropertyChanged += (s, e) =>
                {
                    if(e.PropertyName == "State")
                    {
                        numberOfStateEvents++;
                    }
                };

            _viewModel.SearchString = "Search Expression";
            _viewModel.Search.Execute(null);

            Assert.AreEqual(1, numberOfStateEvents);
            Assert.AreEqual(ViewModelState.Loading, _viewModel.State);
        }

        [TestMethod]
        public void IsSetsTheStateToLoadedWhenTheResultsAreReturned()
        {
            var numberOfStateEvents = 0;
            const string searchExpression = "Search Expression";
            _searchService.SetupSearch(searchExpression);

            _viewModel.SearchString = searchExpression;
            _viewModel.Search.Execute(null);

            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "State")
                {
                    numberOfStateEvents++;
                }
            };

            _searchService.ExecuteSearchCallback(A.ObservableCollection(Mock.Of<ISearchablePromptItem>()));

            Assert.AreEqual(1, numberOfStateEvents);
            Assert.AreEqual(ViewModelState.Loaded, _viewModel.State);
        }
    }
}
