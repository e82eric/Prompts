using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Prompts.Prompting.Construction;
using Prompts.Prompting.Construction.Implementation;
using Prompts.Prompting.ViewModels;
using Prompts.Service.ReportExecution;
using Test.Prompts.Infrastructure;

namespace Test.Prompts.Prompting.Construction.Implementation
{
    [TestClass]
    public class CasscadingSearchShoppingCartBuilderTest
    {
        private CasscadingSearchShoppingCartBuilder _builder;
        private Mock<ICasscadingSearchProvider> _casscadingSearchProvider;
        private Mock<IPromptItemCollectionProvider> _promptItemCollectionProvider;

        [TestInitialize]
        public void Setup()
        {
            _promptItemCollectionProvider = new Mock<IPromptItemCollectionProvider>();
            _casscadingSearchProvider = new Mock<ICasscadingSearchProvider>();
            _builder = new CasscadingSearchShoppingCartBuilder(
                _promptItemCollectionProvider.Object
                , _casscadingSearchProvider.Object);
        }

        [TestMethod]
        public void ItUsesTheDefaultValuesForTheDefaultItems()
        {
            var defaultValuePromptItems = A.ObservableCollection(Mock.Of<ISearchablePromptItem>(), Mock.Of<ISearchablePromptItem>());
            var availableItems = A.ObservableCollection(Mock.Of<ISearchablePromptItem>());

            var promptInfo = A.PromptInfo().Build();

            _promptItemCollectionProvider
                .Setup(
                    b => b.Get(
                        promptInfo.Name,
                        promptInfo.PromptLevelInfo.ParameterName,
                        promptInfo.PromptLevelInfo.AvailableItems))
                .Returns(availableItems);

            _promptItemCollectionProvider
                .Setup(
                    b => b.Get(
                        promptInfo.Name,
                        promptInfo.PromptLevelInfo.ParameterName,
                        promptInfo.DefaultValues))
                .Returns(defaultValuePromptItems);

            var promptToReturn = Mock.Of<IMultiSelectPrompt>();

            _casscadingSearchProvider
                .Setup(p => p.Get(
                    promptInfo.Label, promptInfo.Name, 
                    promptInfo.PromptLevelInfo.ParameterName,
                    availableItems, 
                    defaultValuePromptItems))
                .Returns(promptToReturn);

            var promptReturned = _builder.BuildFrom(promptInfo);
            Assert.AreEqual(promptToReturn, promptReturned);
        }

        [TestMethod]
        public void ItSetsTheSearchStringToTheLavelOfTheDefaultValueIfThereIsOne()
        {
            var promptItems = A.ObservableCollection(Mock.Of<ISearchablePromptItem>(), Mock.Of<ISearchablePromptItem>());
            var defaultValue = A.ValidValue().Build();
            var promptInfo = A.PromptInfo()
                .WithDefaultValues(A.ObservableCollection(defaultValue))
                .Build();

            var availableItems = A.ObservableCollection(Mock.Of<ISearchablePromptItem>());

            _promptItemCollectionProvider
                .Setup(
                    b => b.Get(
                        promptInfo.Name,
                        promptInfo.PromptLevelInfo.ParameterName,
                        promptInfo.PromptLevelInfo.AvailableItems))
                .Returns(availableItems);

            _promptItemCollectionProvider
                .Setup(
                    b => b.Get(
                        promptInfo.Name,
                        promptInfo.PromptLevelInfo.ParameterName,
                        promptInfo.DefaultValues))
                .Returns(promptItems);

            var promptToReturn = new Mock<IMultiSelectPrompt>();

            _casscadingSearchProvider
                .Setup(p => p.Get(
                    promptInfo.Label, promptInfo.Name,
                    promptInfo.PromptLevelInfo.ParameterName,
                    availableItems,
                    promptItems))
                .Returns(promptToReturn.Object);

            _builder.BuildFrom(promptInfo);
            
            promptToReturn.VerifySet(p => p.SearchString = defaultValue.labelField, Times.Exactly(1));
        }

        [TestMethod]
        public void ItDoesNotSetTheSearchStringIfThereIsNotADefaultValue()
        {
            var promptItems = A.ObservableCollection(Mock.Of<ISearchablePromptItem>(), Mock.Of<ISearchablePromptItem>());
            var promptInfo = A.PromptInfo()
                .WithDefaultValues(new ObservableCollection<ValidValue>())
                .Build();

            _promptItemCollectionProvider
                .Setup(
                    b => b.Get(
                        promptInfo.Name,
                        promptInfo.PromptLevelInfo.ParameterName,
                        promptInfo.DefaultValues))
                .Returns(promptItems);

            var promptToReturn = new Mock<IMultiSelectPrompt>();

            _casscadingSearchProvider
                .Setup(p => p.Get(
                    promptInfo.Label, promptInfo.Name,
                    promptInfo.PromptLevelInfo.ParameterName,
                    promptItems,
                    promptItems))
                .Returns(promptToReturn.Object);

            _builder.BuildFrom(promptInfo);

            promptToReturn.VerifySet(p => p.SearchString = It.IsAny<string>(), Times.Exactly(0));
        }
    }
}
