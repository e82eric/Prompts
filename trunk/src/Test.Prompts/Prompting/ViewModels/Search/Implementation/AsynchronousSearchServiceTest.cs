using System;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Prompts.Prompting.ViewModels;
using Prompts.Prompting.ViewModels.Search;
using Prompts.Prompting.ViewModels.Search.Implementation;
using Test.Prompts.Infrastructure;
using Test.Prompts.Infrastructure.Fakes;

namespace Test.Prompts.Prompting.ViewModels.Search.Implementation
{
    [TestClass]
    public class AsynchronousSearchServiceTest
    {
        private Mock<ISearchStringParser<IAsynchronousSearch>> _searchStringParser;
        private AsynchronousSearchService _searchService;
        private FakeAsynchronousSearch _search;

        [TestInitialize]
        public void Setup()
        {
            _searchStringParser = new Mock<ISearchStringParser<IAsynchronousSearch>>();
            _searchService = new AsynchronousSearchService(_searchStringParser.Object);
            _search = new FakeAsynchronousSearch();
        }

        [TestMethod]
        public void ItCallsBackWithTheResultOfSearchReturnedByTheParser2()
        {
            var numberOfCallbacks = 0;

            const string searchExpression = "Search Expression";

            var promptItemsToCallbackWith = A.ObservableCollection(Mock.Of<ISearchablePromptItem>());

            _searchStringParser.Setup(p => p.Parse(searchExpression)).Returns(_search.Object);

            Action<ObservableCollection<ISearchablePromptItem>> searchResultCallback = c =>
            {
                numberOfCallbacks++;
                Assert.AreEqual(promptItemsToCallbackWith, c);
            };

            Action<string> errorCallback = e => { };

            _search.SetupExecute(searchResultCallback, errorCallback);

            _searchService.Search(searchExpression, searchResultCallback, errorCallback);

            Assert.AreEqual(0, numberOfCallbacks);

            _search.ExecuteCallback(promptItemsToCallbackWith);

            Assert.AreEqual(1, numberOfCallbacks);
        }
    }
}
