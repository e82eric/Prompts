using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Prompts.Service.PromptService;
using Prompts.Service.PromptService.Implementation;
using Test.Prompts.Service.Infastructure;

namespace Test.Prompts.Service
{
    [TestFixture]
    public class EmbeddedPromptInfoProviderTest
    {
        private Mock<IStrictDefaultValuesProvider> _strictDefaultValueProvider;
        private Mock<IPromptLevelProvider> _promptLevelProvider;
        private EmbeddedPromptInfoProvider _provider;
        private Mock<IEmptyPromptDefaultValueProvider> _emptyPromptDefaultValueProvider;

        [SetUp]
        public void Setup()
        {
            _strictDefaultValueProvider = new Mock<IStrictDefaultValuesProvider>();
            _promptLevelProvider = new Mock<IPromptLevelProvider>();
            _emptyPromptDefaultValueProvider = new Mock<IEmptyPromptDefaultValueProvider>();

            _provider = new EmbeddedPromptInfoProvider(
                _strictDefaultValueProvider.Object,
                _promptLevelProvider.Object,
                _emptyPromptDefaultValueProvider.Object);
        }

        [Test]
        public void ItGetThePromptNameAndLabelFromTheParameter()
        {
            var parameter = A.ReportParameter()
                .WithName("Parameter Name")
                .WithPrompt("Parameter Prompt")
                .Build();

            var promptInfo = _provider.Get(parameter);
            Assert.AreEqual(parameter.Name, promptInfo.Name);
            Assert.AreEqual(parameter.Prompt, promptInfo.Label);
        }

        [Test]
        public void ItSetsThePromptTypeToEmptyWhenThereAreNoValidValues()
        {
            var parameter = A.ReportParameter()
                .WithValidValues(null)
                .Build();

            var promptInfo = _provider.Get(parameter);
            Assert.AreEqual(PromptType.Empty, promptInfo.PromptType);
        }

        [Test]
        public void ItReturnsShoppingCartWhenTheParameterIsMultiValue()
        {
            var parameter = A.ReportParameter()
                .WithMultiValueFlag(true)
                .Build();

            var promptInfo = _provider.Get(parameter);
            Assert.AreEqual(PromptType.ShoppingCart, promptInfo.PromptType);
        }

        [Test]
        public void ItReturnsDropDownWhenTheParameterIsNotMultiValue()
        {
            var parameter = A.ReportParameter()
                .WithMultiValueFlag(false)
                .Build();

            var promptInfo = _provider.Get(parameter);
            Assert.AreEqual(PromptType.DropDown, promptInfo.PromptType);
        }

        [Test]
        public void ItGetTheDefaultValuesFromTheStrictDefaultValueProviderWhenTheValidValuesAreNotNull()
        {
            var parameter = A.ReportParameter()
                .WithValidValues(A.ValidValue("Value 1"), A.ValidValue("Value 2"))
                .WithDefaultValues("Default 1", "Default 2")
                .Build();

            var promptLevelForProviderToReturn = A.PromptLevel().Build();
            _promptLevelProvider.Setup(p => p.GetPromptLevel(parameter)).Returns(promptLevelForProviderToReturn);

            var defaultValuesFromProviderToReturn = A.Array(A.DefaultValue("Value 1"), A.DefaultValue("Value 2"));
            _strictDefaultValueProvider.Setup(
                p => p.GetDefaultValues(promptLevelForProviderToReturn, parameter.DefaultValues)).Returns(
                    defaultValuesFromProviderToReturn);


            var promptInfo = _provider.Get(parameter);
            Assert.AreEqual(defaultValuesFromProviderToReturn, promptInfo.DefaultValues);
        }

        [Test]
        public void ItGetTheDefaultValuesFromTheEmptyPromptDefaultValueProviderWhenTheValidValuesAreNull()
        {
            var parameter = A.ReportParameter()
                .WithValidValues(null)
                .WithDefaultValues("Default 1", "Default 2")
                .Build();

            var promptLevelForProviderToReturn = A.PromptLevel().Build();
            _promptLevelProvider.Setup(p => p.GetPromptLevel(parameter)).Returns(promptLevelForProviderToReturn);

            var defaultValuesFromProviderToReturn = A.Array(A.DefaultValue("Value 1"), A.DefaultValue("Value 2"));
            _emptyPromptDefaultValueProvider.Setup(
                p => p.Get(parameter)).Returns(
                    defaultValuesFromProviderToReturn);

            var promptInfo = _provider.Get(parameter);
            Assert.AreEqual(defaultValuesFromProviderToReturn, promptInfo.DefaultValues);
        }

        [Test]
        public void ItGetTheDefaultValuesFromTheProviderUsingAnEmptyCollectionWhenParametersDefaultsAreNull()
        {
            var parameter = A.ReportParameter()
                .WithValidValues(A.ValidValue("Value 1"), A.ValidValue("Value 2"))
                .WithDefaultValues(null)
                .Build();

            var promptLevelForProviderToReturn = A.PromptLevel().Build();
            _promptLevelProvider.Setup(p => p.GetPromptLevel(parameter)).Returns(promptLevelForProviderToReturn);

            var defaultValuesFromProviderToReturn = A.Array(A.DefaultValue("Value 1"), A.DefaultValue("Value 2"));
            _strictDefaultValueProvider.Setup(
                p => p.GetDefaultValues(promptLevelForProviderToReturn, It.Is<IEnumerable<string>>(d => d.Count() == 0))).Returns(
                    defaultValuesFromProviderToReturn);

            var promptInfo = _provider.Get(parameter);
            Assert.AreEqual(defaultValuesFromProviderToReturn, promptInfo.DefaultValues);
        }

        [Test]
        public void ItGetsThePromptLevelFromTheProvider()
        {
            var parameter = A.ReportParameter().Build();

            var promptLevel = A.PromptLevel().Build();

            _promptLevelProvider.Setup(p => p.GetPromptLevel(parameter)).Returns(promptLevel);

            var prompt = _provider.Get(parameter);

            Assert.AreEqual(promptLevel, prompt.PromptLevelInfo);
        }
    }
}
