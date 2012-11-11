using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Prompts.Prompting.Model;
using Prompts.Prompting.ViewModels.Search;
using Prompts.Prompting.ViewModels.Search.Implementation;

namespace Test.Prompts.Prompting.ViewModels.Search.Implementation
{
    [TestClass]
    public class SearchStringParserTest
    {
        private Mock<ISearchProvider<ISearch>> _searchProvider;
        private SearchStringParser<ISearch> _parser;
        private const string SpecialCharacter = "*";

        [TestInitialize]
        public void Setup()
        {
            _searchProvider = new Mock<ISearchProvider<ISearch>>();
            _parser = new SearchStringParser<ISearch>(SpecialCharacter, _searchProvider.Object);
        }

        [TestMethod]
        public void ItReturnsAnNullSearchWhenTheSearchStringIsJustTheSpecialCharacter()
        {
            var searchToReturn = new Mock<ISearch>().Object;
          
            _searchProvider.Setup(p => p.CreateNullSearch()).Returns(searchToReturn);  

            Assert.AreEqual(searchToReturn, _parser.Parse(SpecialCharacter));
        }

        [TestMethod]
        public void ItReturnsAnNullSearchWhenTheSearchStringIsBlank()
        {
            var searchToReturn = new Mock<ISearch>().Object;

            _searchProvider.Setup(p => p.CreateNullSearch()).Returns(searchToReturn);

            Assert.AreEqual(searchToReturn, _parser.Parse(""));
        }

        [TestMethod]
        public void ItReturnsAnContainsSearchWhenTheSearchStringBeginsWithAndEndsWithTheSpecialCharacter()
        {
            const string searchExpression = "*Search String*";

            var searchToReturn = new Mock<ISearch>().Object;

            _searchProvider.Setup(p => p.CreateContainsSearch("Search String")).Returns(searchToReturn);

            Assert.AreEqual(searchToReturn, _parser.Parse(searchExpression));
        }

        [TestMethod]
        public void ItReturnsAnStartsWithSearchWhenTheSearchExpressionEndsWithTheSpecialCharacter()
        {
            const string searchExpression = "Search String*";

            var searchToReturn = new Mock<ISearch>().Object;

            _searchProvider.Setup(p => p.CreateStartsWithSearch("Search String")).Returns(searchToReturn);

            Assert.AreEqual(searchToReturn, _parser.Parse(searchExpression));
        }

        [TestMethod]
        public void ItReturnsAnEndsWithSearchWhenTheSearchExpressionStartsWithTheSpecialCharacter()
        {
            const string searchExpression = "*Search String";

            var searchToReturn = new Mock<ISearch>().Object;

            _searchProvider.Setup(p => p.CreateEndsWithSearch("Search String")).Returns(searchToReturn);

            Assert.AreEqual(searchToReturn, _parser.Parse(searchExpression));
        }

        [TestMethod]
        public void ItReturnsAnEqualsSearchWhenTheSearchExpressionDoesNotStartWithOrEndWithTheSpecialCharacter()
        {
            const string searchExpression = "Search String";

            var searchToReturn = new Mock<ISearch>().Object;

            _searchProvider.Setup(p => p.CreateEqualsSearch("Search String")).Returns(searchToReturn);

            Assert.AreEqual(searchToReturn, _parser.Parse(searchExpression));
        }
    }
}