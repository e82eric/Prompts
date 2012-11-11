using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Prompts.Prompting.Model;
using Prompts.Prompting.ViewModels;
using Prompts.Prompting.ViewModels.Implementation;
using Prompts.Prompting.ViewModels.Search;
using Test.Prompts.Infrastructure;

namespace Test.Prompts.Prompting.ViewModels.Implementation
{
    [TestClass]
    public class MultiSelectSearchTest
    {
        private Mock<ISearchService> _searchService;

        [TestInitialize]
        public void Setup()
        {
            _searchService = new Mock<ISearchService>();
        }

        [TestMethod]
        public void ItSetsTheAvailableItemsToTheSearchServiceGetAllWhenTheShoppingCartIsConstructed()
        {
            var expected = A.ObservableCollection(Mock.Of<ISearchablePromptItem>());
    
            _searchService
                .Setup(s => s.GetAll())
                .Returns(expected);

            var shoppingCart = new MultiSelectSearch(
                "label", 
                "name", 
                _searchService.Object, 
                new ObservableCollection<ISearchablePromptItem>());

            Assert.AreEqual(expected, shoppingCart.AvailableItems);
        }

        [TestMethod]
        public void ItSetsTheAvailableItemsToTheResultOfTheSearchServiceWhenTheSearchCommandIsExecuted()
        {
            var expected = A.ObservableCollection(Mock.Of<ISearchablePromptItem>());

            const string searchString = "Search String";

            _searchService
                .Setup(s => s.Search(searchString))
                .Returns(expected);

            var shoppingCart= new MultiSelectSearch(
                "label", 
                "name", 
                _searchService.Object, 
                new ObservableCollection<ISearchablePromptItem>());

            shoppingCart.SearchString = searchString;

            shoppingCart.Search.Execute(null);

            Assert.AreEqual(expected, shoppingCart.AvailableItems);
        }

        [TestMethod]
        public void ItIsCreatedWithAnEmptySearchString()
        {
            var shoppingCart = new MultiSelectSearch(
                "Label", 
                "Name", 
                _searchService.Object, 
                new ObservableCollection<ISearchablePromptItem>());

            Assert.AreEqual(string.Empty, shoppingCart.SearchString);
        }
    }
}