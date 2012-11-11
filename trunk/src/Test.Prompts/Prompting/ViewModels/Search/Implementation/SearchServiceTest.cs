using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Prompts.Prompting.ViewModels;
using Prompts.Prompting.ViewModels.Search;
using Prompts.Prompting.ViewModels.Search.Implementation;
using Test.Prompts.Infrastructure;

namespace Test.Prompts.Prompting.ViewModels.Search.Implementation
{
    [TestClass]
    public class SearchServiceTest
    {
        private Mock<ISearchStringParser<ISearch>> _searchStringParser;
        private Mock<ISearchablePromptItemCollection> _searchablePromptItemCollection;
        private SearchService _searchService;

        [TestInitialize]
        public void Setup()
        {
            _searchStringParser = new Mock<ISearchStringParser<ISearch>>();
            _searchablePromptItemCollection = new Mock<ISearchablePromptItemCollection>();
            _searchService = new SearchService(_searchablePromptItemCollection.Object, _searchStringParser.Object);
        }

        [TestMethod]
        public void ItReturnsThePromptItemCollectionsEnumberationForGetAll()
        {
            var enumerationToReturn = A.ObservableCollection(new Mock<ISearchablePromptItem>().Object);

            _searchablePromptItemCollection
                .SetupGet(c => c.Enumeration)
                .Returns(enumerationToReturn);

            Assert.AreEqual(enumerationToReturn, _searchService.GetAll());
        }

        [TestMethod]
        public void ItReturnsTheResultOfTheSearchReturnedByTheParserForSearch()
        {
            const string searchExpression = "Search String Stub";

            var searchToReturn = new Mock<ISearch>();

            var expected = A.ObservableCollection(Mock.Of<ISearchablePromptItem>());

            searchToReturn
                .Setup(s => s.Execute(_searchablePromptItemCollection.Object))
                .Returns(expected);

            _searchStringParser
                .Setup(p => p.Parse(searchExpression))
                .Returns(searchToReturn.Object);

            var actual = _searchService.Search(searchExpression);

            Assert.AreEqual(expected, actual);
        }
    }
}