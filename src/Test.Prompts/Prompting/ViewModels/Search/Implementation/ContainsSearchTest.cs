using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Prompts.Prompting.Model;
using Prompts.Prompting.ViewModels;
using Prompts.Prompting.ViewModels.Search;
using Prompts.Prompting.ViewModels.Search.Implementation;

namespace Test.Prompts.Prompting.ViewModels.Search.Implementation
{
    [TestClass]
    public class ContainsSearchTest
    {
        [TestMethod]
        public void Execute()
        {
            const string searchString = "Search String";

            var fakedPromptItems = new ObservableCollection<ISearchablePromptItem>(new[] { new Mock<ISearchablePromptItem>().Object, new Mock<ISearchablePromptItem>().Object });
            var fakeSearchablePromptValueCollection = new Mock<ISearchablePromptItemCollection>();
            fakeSearchablePromptValueCollection.Setup(c => c.LabelContains(searchString)).Returns(fakedPromptItems);

            var containsSearch = new ContainsSearch(searchString);

            var searchResults = containsSearch.Execute(fakeSearchablePromptValueCollection.Object);

            Assert.AreEqual(fakedPromptItems, searchResults);
        }
    }
}
