using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Prompts.Prompting.Construction;
using Prompts.Prompting.Construction.Implementation;
using Prompts.Prompting.ViewModels;
using Test.Prompts.Infrastructure;

namespace Test.Prompts.Prompting.Construction.Implementation
{
    [TestClass]
    public class CasscadingSearchShoppingCartBuilder2Test
    {
        private CasscadingSearchShoppingCartBuilder _builder;
        private Mock<IPromptBuilder> _promptBuilder;

        [TestInitialize]
        public void Setup()
        {
            _promptBuilder = new Mock<IPromptBuilder>();
            _builder = new CasscadingSearchShoppingCartBuilder(_promptBuilder.Object);
        }

        [TestMethod]
        public void ItSetsTheSearchStringToTheLabelOfTheDefaultValueIfThereIsOne()
        {
            const string searchString = "Search String 1";

            var defaultValue = Mock.Of<ISearchablePromptItem>(v => v.Label == searchString);

            var promptInfo = A.PromptInfo().Build();

            var promptToReturn = new Mock<IMultiSelectPrompt>();

            promptToReturn.Setup(p => p.SelectedItems).Returns(A.ObservableCollection(defaultValue));

            _promptBuilder
                .Setup(p => p.BuildFrom(promptInfo))
                .Returns(promptToReturn.Object);

            var prompt = _builder.BuildFrom(promptInfo);

            promptToReturn.VerifySet(p => p.SearchString = searchString, Times.Exactly(1));
            Assert.AreEqual(promptToReturn.Object, prompt);
        }

        [TestMethod]
        public void ItDoesNotSetTheSearchStringIfThereIsNotADefaultValue()
        {
            var promptInfo = A.PromptInfo().Build();

            var promptToReturn = new Mock<IMultiSelectPrompt>();

            promptToReturn.Setup(p => p.SelectedItems).Returns(new ObservableCollection<ISearchablePromptItem>());

            _promptBuilder
                .Setup(p => p.BuildFrom(promptInfo))
                .Returns(promptToReturn.Object);

            var prompt = _builder.BuildFrom(promptInfo);

            promptToReturn.VerifySet(p => p.SearchString = It.IsAny<string>(), Times.Exactly(0));
            Assert.AreEqual(promptToReturn.Object, prompt);
        }
    }
}