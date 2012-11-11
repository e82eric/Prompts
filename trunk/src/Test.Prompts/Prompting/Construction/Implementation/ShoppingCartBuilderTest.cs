using System;
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
    public class ShoppingCartBuilderTest
    {
        private Mock<IPromptItemProvider<IPromptItem>> _promptItemProvider;
        private ShoppingCartBuilder<IPromptItem> _builder;
        private Mock<IMultiSelectPromptProvider<IPromptItem>> _shoppingCartProvider;

        [TestInitialize]
        public void Setup()
        {
            _promptItemProvider = new Mock<IPromptItemProvider<IPromptItem>>();
            _shoppingCartProvider = new Mock<IMultiSelectPromptProvider<IPromptItem>>();
            _builder = new ShoppingCartBuilder<IPromptItem>(_promptItemProvider.Object, _shoppingCartProvider.Object);
        }

        [TestMethod]
        public void ItUsesThePromptItemProviderToGetTheAvailableItems()
        {
            const string promptName = "Prompt Name";
            const string label = "Prompt Label";
            const string parameterName = "Parameter Name";

            var validValue1 = A.ValidValue().Build();
            var validValue2 = A.ValidValue().Build();
            var validValue3 = A.ValidValue().Build();
            var validValues = A.Array(validValue1, validValue2, validValue3);

            var promptItem1 = Mock.Of<ISearchablePromptItem>();
            var promptItem2 = Mock.Of<ISearchablePromptItem>();
            var promptItem3 = Mock.Of<ISearchablePromptItem>();

            _promptItemProvider.Setup(p => p.Get(promptName, parameterName, validValue1)).Returns(promptItem1);
            _promptItemProvider.Setup(p => p.Get(promptName, parameterName, validValue2)).Returns(promptItem2);
            _promptItemProvider.Setup(p => p.Get(promptName, parameterName, validValue3)).Returns(promptItem3);

            var promptLevel = A.PromptLevel().WithAvailableItems(validValues).WithParameterName(parameterName).Build();
            var promptInfo = A.PromptInfo()
                .WithName(promptName)
                .WithLabel(label)
                .WithPromptLevel(promptLevel).Build();

            var promptToReturn = Mock.Of<IPrompt>();

            Func<ObservableCollection<IPromptItem>, bool> availableItemMatch = a =>
                {
                    a.AssertLength(3);
                    a.AssertContains(promptItem1, promptItem2, promptItem3);
                    return true;
                };

            _shoppingCartProvider
                .Setup(
                    p => 
                    p.Get(label, promptName, It.Is<ObservableCollection<IPromptItem>>(a => availableItemMatch(a)), It.IsAny<ObservableCollection<IPromptItem>>()))
                    .Returns(promptToReturn);

            var promptReturned = _builder.BuildFrom(promptInfo);
            Assert.AreEqual(promptToReturn, promptReturned);
        }

        [TestMethod]
        public void ItSetsTheDefaultItemsToAnEmptyCollectionWhenThereAreNoDefaultValues()
        {
            const string promptName = "Prompt Name";
            const string parameterName = "Parameter Name";
            const string label = "Prompt Label";

            var validValue1 = A.ValidValue().Build();
            var validValue2 = A.ValidValue().Build();
            var validValues = A.Array(validValue1, validValue2);

            var promptItem1 = Mock.Of<ISearchablePromptItem>();
            var promptItem2 = Mock.Of<ISearchablePromptItem>();

            _promptItemProvider.Setup(p => p.Get(promptName, parameterName, validValue1)).Returns(promptItem1);
            _promptItemProvider.Setup(p => p.Get(promptName, parameterName, validValue2)).Returns(promptItem2);

            var promptLevel = A.PromptLevel()
                .WithAvailableItems(validValues)
                .WithParameterName(parameterName)
                .Build();
            var promptInfo = A.PromptInfo()
                .WithName(promptName)
                .WithLabel(label)
                .WithPromptLevel(promptLevel)
                .WithDefaultValues(new ObservableCollection<ValidValue>())
                .Build();

            var promptToReturn = Mock.Of<IPrompt>();

            Func<ObservableCollection<IPromptItem>, bool> defaultItemMatach = i =>
                {
                    i.AssertLength(0);
                    return true;
                };

            _shoppingCartProvider
                .Setup(p => p.Get(
                    label
                    , promptName
                    , It.IsAny<ObservableCollection<IPromptItem>>()
                    , It.Is<ObservableCollection<IPromptItem>>(a => defaultItemMatach(a))))
                .Returns(promptToReturn);

            var promptReturned = _builder.BuildFrom(promptInfo);
            Assert.AreEqual(promptToReturn, promptReturned);
        }

        [TestMethod]
        public void ItUsesTheValueFieldToIdentifyTheSelectedItems()
        {
            const string promptName = "Prompt Name";
            const string parameterName = "Parameter Name";
            const string label = "Label";
            const string defaultValueValue = "Value";

            var validValue1 = A.ValidValue().WithValue("Value 1").Build();
            var validValue2 = A.ValidValue().WithValue(defaultValueValue).Build();
            var validValues = A.Array(validValue1, validValue2);

            var promptItem1 = Mock.Of<ISearchablePromptItem>();
            var promptItem2 = Mock.Of<ISearchablePromptItem>();

            _promptItemProvider.Setup(p => p.Get(promptName, parameterName, validValue1)).Returns(promptItem1);
            _promptItemProvider.Setup(p => p.Get(promptName, parameterName, validValue2)).Returns(promptItem2);

            var defaultValidValues = A.ObservableCollection(A.ValidValue().WithValue(defaultValueValue).Build());

            var promptLevel = A.PromptLevel()
                .WithAvailableItems(validValues)
                .WithParameterName(parameterName)
                .Build();
            var promptInfo = A.PromptInfo()
                .WithName(promptName)
                .WithPromptLevel(promptLevel)
                .WithLabel(label)
                .WithDefaultValues(defaultValidValues)
                .Build();

            var promptToReturn = Mock.Of<IPrompt>();

            Func<ObservableCollection<IPromptItem>, bool> defaultItemMatach = i =>
            {
                i.AssertLength(1);
                i.AssertContains(promptItem2);
                return true;
            };

            _shoppingCartProvider
                .Setup(p => p.Get(
                    label
                    , promptName
                    , It.IsAny<ObservableCollection<IPromptItem>>()
                    , It.Is<ObservableCollection<IPromptItem>>(a => defaultItemMatach(a))))
                .Returns(promptToReturn);

            var promptReturned = _builder.BuildFrom(promptInfo);
            Assert.AreEqual(promptToReturn, promptReturned);
        }

        [TestMethod]
        public void ItCanIdentifyMultipleSelectedItems()
        {
            const string promptName = "Prompt Name";
            const string parameterName = "Parameter Name";
            const string label = "Label";
            const string defaultValueValue1 = "Value 1";
            const string defaultValueValue2 = "Value 2";

            var validValue1 = A.ValidValue().WithValue(defaultValueValue1).Build();
            var validValue2 = A.ValidValue().WithValue("Not Default").Build();
            var validValue3 = A.ValidValue().WithValue(defaultValueValue2).Build();
            var validValues = A.Array(validValue1, validValue2, validValue3);

            var promptItem1 = Mock.Of<ISearchablePromptItem>();
            var promptItem2 = Mock.Of<ISearchablePromptItem>();
            var promptItem3 = Mock.Of<ISearchablePromptItem>();

            _promptItemProvider.Setup(p => p.Get(promptName, parameterName, validValue1)).Returns(promptItem1);
            _promptItemProvider.Setup(p => p.Get(promptName, parameterName, validValue2)).Returns(promptItem2);
            _promptItemProvider.Setup(p => p.Get(promptName, parameterName, validValue3)).Returns(promptItem3);

            var defaultValidValues = A.ObservableCollection(
                A.ValidValue().WithValue(defaultValueValue1).Build(),
                A.ValidValue().WithValue(defaultValueValue2).Build());

            var promptLevel = A.PromptLevel()
                .WithAvailableItems(validValues)
                .WithParameterName(parameterName)
                .Build();
            var promptInfo = A.PromptInfo()
                .WithName(promptName)
                .WithPromptLevel(promptLevel)
                .WithDefaultValues(defaultValidValues)
                .Build();

            var promptToReturn = Mock.Of<IPrompt>();

            Func<ObservableCollection<IPromptItem>, bool> defaultItemMatach = i =>
            {
                i.AssertLength(2);
                i.AssertContains(promptItem1, promptItem3);
                return true;
            };

            _shoppingCartProvider
                .Setup(p => p.Get(
                    label
                    , promptName
                    , It.IsAny<ObservableCollection<IPromptItem>>()
                    , It.Is<ObservableCollection<IPromptItem>>(a => defaultItemMatach(a))))
                .Returns(promptToReturn);

            var promptReturned = _builder.BuildFrom(promptInfo);
            Assert.AreEqual(promptToReturn, promptReturned);
        }
    }
}
