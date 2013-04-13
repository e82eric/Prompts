using System;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Prompts.Prompting.Construction;
using Prompts.Prompting.Construction.Implementation;
using Prompts.Prompting.ViewModels;
using Prompts.Service.ReportExecution;
using Test.Prompts.Infrastructure;
using DefaultValue = Prompts.Service.PromptService.DefaultValue;

namespace Test.Prompts.Prompting.Construction.Implementation
{
    [TestClass]
    public class ShoppingCartBuilder2Test
    {
        private Mock<IPromptItemProvider<IPromptItem>> _promptItemProvider;
        private ShoppingCartBuilder<IPromptItem> _builder;
        private Mock<IShoppingCartProvider<IPromptItem>> _shoppingCartProvider;

        [TestInitialize]
        public void Setup()
        {
            _promptItemProvider = new Mock<IPromptItemProvider<IPromptItem>>();
            _shoppingCartProvider = new Mock<IShoppingCartProvider<IPromptItem>>();
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

            var promptToReturn = Mock.Of<IMultiSelectPrompt>();

            Func<ObservableCollection<IPromptItem>, bool> availableItemMatch = a =>
                {
                    a.AssertLength(3);
                    a.AssertContains(promptItem1, promptItem2, promptItem3);
                    return true;
                };

            _shoppingCartProvider
                .Setup(
                    p =>
                    p.Get(
                        promptInfo,
                        It.Is<ObservableCollection<IPromptItem>>(a => availableItemMatch(a)),
                        It.IsAny<ObservableCollection<IPromptItem>>()))
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
                .WithDefaultValues(new ObservableCollection<DefaultValue>())
                .Build();

            var promptToReturn = Mock.Of<IMultiSelectPrompt>();

            Func<ObservableCollection<IPromptItem>, bool> defaultItemMatach = i =>
                {
                    i.AssertLength(0);
                    return true;
                };

            _shoppingCartProvider
                .Setup(p => p.Get(
                    promptInfo
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

            var defaultValidValues = A.ObservableCollection(A.DefaultValue().WithValue(defaultValueValue).Build());

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

            var promptToReturn = Mock.Of<IMultiSelectPrompt>();

            Func<ObservableCollection<IPromptItem>, bool> defaultItemMatach = i =>
                {
                    i.AssertLength(1);
                    i.AssertContains(promptItem2);
                    return true;
                };

            _shoppingCartProvider
                .Setup(p => p.Get(
                    promptInfo
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
                A.DefaultValue().WithValue(defaultValueValue1).Build(),
                A.DefaultValue().WithValue(defaultValueValue2).Build());

            var promptLevel = A.PromptLevel()
                .WithAvailableItems(validValues)
                .WithParameterName(parameterName)
                .Build();
            var promptInfo = A.PromptInfo()
                .WithName(promptName)
                .WithPromptLevel(promptLevel)
                .WithDefaultValues(defaultValidValues)
                .Build();

            var promptToReturn = Mock.Of<IMultiSelectPrompt>();

            Func<ObservableCollection<IPromptItem>, bool> defaultItemMatach = i =>
                {
                    i.AssertLength(2);
                    i.AssertContains(promptItem1, promptItem3);
                    return true;
                };

            _shoppingCartProvider
                .Setup(p => p.Get(
                    promptInfo
                    , It.IsAny<ObservableCollection<IPromptItem>>()
                    , It.Is<ObservableCollection<IPromptItem>>(a => defaultItemMatach(a))))
                .Returns(promptToReturn);

            var promptReturned = _builder.BuildFrom(promptInfo);
            Assert.AreEqual(promptToReturn, promptReturned);
        }

        [TestMethod]
        public void ItSetsTheDefaultAllMemberPropertyToTrueWhenTheDefaultIsAllMemberIsTrue()
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

            var defaultValidValues = A.ObservableCollection(A.DefaultValue().WithValue(defaultValueValue).WithIsAllMember(true).Build());

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

            var promptToReturn = Mock.Of<IMultiSelectPrompt>();

            Func<ObservableCollection<IPromptItem>, bool> defaultItemMatach = i =>
                {
                    i.AssertLength(1);
                    i.AssertContains(promptItem2);
                    Assert.IsTrue(promptItem2.IsDefaultAll);
                    return true;
                };

            _shoppingCartProvider
                .Setup(p => p.Get(
                    promptInfo
                    , It.IsAny<ObservableCollection<IPromptItem>>()
                    , It.Is<ObservableCollection<IPromptItem>>(a => defaultItemMatach(a))))
                .Returns(promptToReturn);

            var promptReturned = _builder.BuildFrom(promptInfo);
            Assert.AreEqual(promptToReturn, promptReturned);
        }

        [TestMethod]
        public void ItSetsTheDefaultAllMemberPropertyToFalseWhenTheDefaultIsAllMemberIsFalse()
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

            var defaultValidValues = A.ObservableCollection(A.DefaultValue().WithValue(defaultValueValue).WithIsAllMember(false).Build());

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

            var promptToReturn = Mock.Of<IMultiSelectPrompt>();

            Func<ObservableCollection<IPromptItem>, bool> defaultItemMatach = i =>
                {
                    i.AssertLength(1);
                    i.AssertContains(promptItem2);
                    Assert.IsFalse(promptItem2.IsDefaultAll);
                    return true;
                };

            _shoppingCartProvider
                .Setup(p => p.Get(
                    promptInfo
                    , It.IsAny<ObservableCollection<IPromptItem>>()
                    , It.Is<ObservableCollection<IPromptItem>>(a => defaultItemMatach(a))))
                .Returns(promptToReturn);

            var promptReturned = _builder.BuildFrom(promptInfo);
            Assert.AreEqual(promptToReturn, promptReturned);
        }

        [TestMethod]
        public void ItWillCanSetTheIsAllMemberPropertyOnMultipleItems()
        {
            const string promptName = "Prompt Name";
            const string parameterName = "Parameter Name";
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
                A.DefaultValue().WithValue(defaultValueValue1).WithIsAllMember(true).Build(),
                A.DefaultValue().WithValue(defaultValueValue2).WithIsAllMember(true).Build());

            var promptLevel = A.PromptLevel()
                .WithAvailableItems(validValues)
                .WithParameterName(parameterName)
                .Build();
            var promptInfo = A.PromptInfo()
                .WithName(promptName)
                .WithPromptLevel(promptLevel)
                .WithDefaultValues(defaultValidValues)
                .Build();

            var promptToReturn = Mock.Of<IMultiSelectPrompt>();

            Func<ObservableCollection<IPromptItem>, bool> defaultItemMatach = i =>
                {
                    i.AssertLength(2);
                    i.AssertContains(promptItem1, promptItem3);
                    Assert.IsTrue(promptItem1.IsDefaultAll);
                    Assert.IsTrue(promptItem3.IsDefaultAll);
                    Assert.IsFalse(promptItem2.IsDefaultAll);
                    return true;
                };

            _shoppingCartProvider
                .Setup(p => p.Get(
                    promptInfo
                    , It.IsAny<ObservableCollection<IPromptItem>>()
                    , It.Is<ObservableCollection<IPromptItem>>(a => defaultItemMatach(a))))
                .Returns(promptToReturn);

            var promptReturned = _builder.BuildFrom(promptInfo);
            Assert.AreEqual(promptToReturn, promptReturned);
        }
    }
}