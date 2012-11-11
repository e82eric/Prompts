using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Prompts.Prompting.Model;
using Prompts.Prompting.ViewModels;
using Prompts.Prompting.ViewModels.Search;
using Prompts.Prompting.ViewModels.Search.Implementation;
using Test.Prompts.Infrastructure;

namespace Test.Prompts.Prompting.ViewModels.Search.Implementation
{
    [TestClass]
    public class StartsWithSearchTest
    {
        [TestMethod]
        public void Execute()
        {
            const string searchString = "Search String";

            var fakedPromptItems = A.ObservableCollection(
                    Mock.Of<ISearchablePromptItem>(), 
                    Mock.Of<ISearchablePromptItem>());
            
            var fakeSearchablePromptValueCollection = new Mock<ISearchablePromptItemCollection>();
            
            fakeSearchablePromptValueCollection
                .Setup(c => c.LabelStartsWith(searchString))
                .Returns(fakedPromptItems);

            var search = new StartsWithSearch(searchString);

            var searchResults = search.Execute(fakeSearchablePromptValueCollection.Object);

            Assert.AreEqual(fakedPromptItems, searchResults);
        }
    }
}
