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
    public class SingleSelectPromptBuilderTest
    {
        private Mock<IPromptItemProvider<IPromptItem>> _promptItemProvider;
        private SingleSelectPromptBuilder<IPromptItem> _builder;
        private Mock<ISingleSelectPromptProvider<IPromptItem>> _promptProvider;

        [TestInitialize]
        public void Setup()
        {
            _promptItemProvider = new Mock<IPromptItemProvider<IPromptItem>>();
            _promptProvider = new Mock<ISingleSelectPromptProvider<IPromptItem>>();
            _builder = new SingleSelectPromptBuilder<IPromptItem>(
                _promptItemProvider.Object,
                _promptProvider.Object);
        }

        [TestMethod]
        public void ItSendsTheCorrectAvailableItemsToThePromptProvider()
        {
            const string promptName = "Prompt Name";
            const string parameterName = "Parameter Name";

            var validValue1 = A.ValidValue().Build();
            var validValue2 = A.ValidValue().Build();
            var validValue3 = A.ValidValue().Build();
            var availableItems = A.Array(validValue1, validValue2, validValue3);

            var promptLevel = A.PromptLevel()
                .WithAvailableItems(availableItems)
                .WithParameterName(parameterName)
                .Build();

            var promptInfo = A.PromptInfo().WithName(promptName).WithPromptLevel(promptLevel).Build();

            var promptItem1 = Mock.Of<IPromptItem>();
            var promptItem2 = Mock.Of<IPromptItem>();
            var promptItem3 = Mock.Of<IPromptItem>();

            _promptItemProvider.Setup(p => p.Get(promptName, parameterName, validValue1)).Returns(promptItem1);
            _promptItemProvider.Setup(p => p.Get(promptName, parameterName, validValue2)).Returns(promptItem2);
            _promptItemProvider.Setup(p => p.Get(promptName, parameterName, validValue3)).Returns(promptItem3);

            var promptToReturn = Mock.Of<IPrompt>();

            Func<ObservableCollection<IPromptItem>, bool> availableItemMatch = i =>
                {
                    i.AssertLength(3);
                    i.AssertContains(promptItem1, promptItem2, promptItem3);

                    return true;
                };

            _promptProvider
                .Setup(
                    p =>
                    p.Get(promptName
                          , promptInfo.Label
                          , It.Is<ObservableCollection<IPromptItem>>(a => availableItemMatch(a))
                          , It.IsAny<IPromptItem>()))
                .Returns(promptToReturn);

            var promptReturned = _builder.BuildFrom(promptInfo);

            Assert.AreEqual(promptToReturn, promptReturned);
        }

        [TestMethod]
        public void ItNullToThePromptProviderWhenTheDefaultValuesAreEmpty()
        {
            const string promptName = "Prompt Name";
            const string parameterName = "Parameter Name";

            const string defaultValueValue = "Value 2";

            var validValue1 = A.ValidValue().WithValue("Value 1").Build();
            var validValue2 = A.ValidValue().WithValue(defaultValueValue).Build();
            var availableItems = A.Array(validValue1, validValue2);

            var promptLevel = A.PromptLevel()
                .WithAvailableItems(availableItems)
                .WithParameterName(parameterName)
                .Build();

            var promptInfo = A.PromptInfo()
                .WithName(promptName)
                .WithDefaultValues(new ObservableCollection<DefaultValue>())
                .WithPromptLevel(promptLevel).Build();

            var promptItem1 = Mock.Of<IPromptItem>();
            var promptItem2 = Mock.Of<IPromptItem>();

            _promptItemProvider.Setup(p => p.Get(promptName, parameterName, validValue1)).Returns(promptItem1);
            _promptItemProvider.Setup(p => p.Get(promptName, parameterName, validValue2)).Returns(promptItem2);

            var promptToReturn = Mock.Of<IPrompt>();

            _promptProvider
                .Setup(
                    p =>
                    p.Get(promptName, promptInfo.Label, It.IsAny<ObservableCollection<IPromptItem>>(),
                          It.Is<IPromptItem>(i => i == null)))
                .Returns(promptToReturn);

            var promptReturned = _builder.BuildFrom(promptInfo);

            Assert.AreEqual(promptToReturn, promptReturned);
        }

        [TestMethod]
        public void ItThrowsAnExceptionWhenThereIsMoreThanOneDefaultValue()
        {
            const string promptName = "Prompt Name";
            const string parameterName = "Parameter Name";

            const string defaultValueValue = "Value 2";

            var validValue1 = A.ValidValue().WithValue("Value 1").Build();
            var validValue2 = A.ValidValue().WithValue(defaultValueValue).Build();
            var availableItems = A.Array(validValue1, validValue2);

            var defaultValue1 = A.DefaultValue().WithValue("Value 1").Build();
            var defaultValue2 = A.DefaultValue().WithValue(defaultValueValue).Build();

            var promptLevel = A.PromptLevel()
                .WithAvailableItems(availableItems)
                .WithParameterName(parameterName)
                .Build();

            var promptInfo = A.PromptInfo()
                .WithName(promptName)
                .WithDefaultValues(A.ObservableCollection(defaultValue1, defaultValue2))
                .WithPromptLevel(promptLevel).Build();

            var promptItem1 = Mock.Of<IPromptItem>();
            var promptItem2 = Mock.Of<IPromptItem>();

            _promptItemProvider.Setup(p => p.Get(promptName, parameterName, validValue1)).Returns(promptItem1);
            _promptItemProvider.Setup(p => p.Get(promptName, parameterName, validValue2)).Returns(promptItem2);

            ExceptionAssert.Throws<DropDownBuilderException>(() => _builder.BuildFrom(promptInfo));
        }

        [TestMethod]
        public void ItSendsTheCorrectDefaultPromptItemToThePromptProvider()
        {
            const string promptName = "Prompt Name";
            const string parameterName = "Parameter Name";

            const string defaultValueValue = "Value 2";

            var validValue1 = A.ValidValue().WithValue("Value 1").Build();
            var validValue2 = A.ValidValue().WithValue(defaultValueValue).Build();
            var availableItems = A.Array(validValue1, validValue2);

            var promptLevel = A.PromptLevel()
                .WithAvailableItems(availableItems)
                .WithParameterName(parameterName)
                .Build();

            var promptInfo = A.PromptInfo()
                .WithName(promptName)
                .WithDefaultValues(A.ObservableCollection(A.DefaultValue().WithValue(defaultValueValue).Build()))
                .WithPromptLevel(promptLevel).Build();

            var promptItem1 = Mock.Of<IPromptItem>();
            var promptItem2 = Mock.Of<IPromptItem>();

            _promptItemProvider.Setup(p => p.Get(promptName, parameterName, validValue1)).Returns(promptItem1);
            _promptItemProvider.Setup(p => p.Get(promptName, parameterName, validValue2)).Returns(promptItem2);

            var promptToReturn = Mock.Of<IPrompt>();

            _promptProvider
                .Setup(p =>
                       p.Get(promptName, promptInfo.Label, It.IsAny<ObservableCollection<IPromptItem>>(),
                             It.Is<IPromptItem>(i => i == promptItem2)))
                .Returns(promptToReturn);

            var promptReturned = _builder.BuildFrom(promptInfo);

            Assert.AreEqual(promptToReturn, promptReturned);
        }
    }
}
