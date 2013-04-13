using System.Linq;
using Moq;
using NUnit.Framework;
using Prompts.Service.PromptService.Implementation;
using Test.Prompts.Service.Infastructure;
using Prompts.Service.PromptService;

namespace Test.Prompts.Service
{
    [TestFixture]
    public class EmptyPromptDefaultValueProviderTest
    {
        private EmptyPromptDefaultValueProvider _emptyPromptDefaultValueProvider;
        private Mock<IDefaultValueProvider> _defaultValueProvider;

        [SetUp]
        public void Setup()
        {
            _defaultValueProvider = new Mock<IDefaultValueProvider>();
            _emptyPromptDefaultValueProvider = new EmptyPromptDefaultValueProvider(_defaultValueProvider.Object);
        }

        [Test]
        public void ItSetsTheDefaultValueToTheDefaultValueWhenThereIsOne()
        {
            const string defaultString = "Default";

            var parameter = A.ReportParameter()
                .WithName("Name")
                .WithPrompt("Label")
                .WithDefaultValues(defaultString)
                .Build();

            var defaultValue = A.DefaultValue("Default1");
            _defaultValueProvider.Setup(p => p.Get(defaultString, string.Empty)).Returns(defaultValue);

            var defaultValues = _emptyPromptDefaultValueProvider.Get(parameter);

            defaultValues.AssetItemsAndLength(defaultValue);
        }

        [Test]
        public void ItSetsTheDefaultValueToAnEmptyCollectionWhenThereAreNoDefaults()
        {
            var parameter = A.ReportParameter()
                .WithName("Name")
                .WithPrompt("Label")
                .WithDefaultValues(null)
                .Build();

            var defaultValues = _emptyPromptDefaultValueProvider.Get(parameter);

            Assert.AreEqual(0, defaultValues.Count());
        }

        [Test]
        public void ItSetsTheDefaultValueToTheFirstWhenThereAreMoreThanOne()
        {
            const string default1 = "Default 1";
            const string default2 = "Default 2";
            const string default3 = "Default 3";

            var defaultValue1 = A.DefaultValue("Default1");
            var defaultValue2 = A.DefaultValue("Default2");
            var defaultValue3 = A.DefaultValue("Default3");
            _defaultValueProvider.Setup(p => p.Get(default1, string.Empty)).Returns(defaultValue1);
            _defaultValueProvider.Setup(p => p.Get(default2, string.Empty)).Returns(defaultValue2);
            _defaultValueProvider.Setup(p => p.Get(default3, string.Empty)).Returns(defaultValue3);

            var parameter = A.ReportParameter()
                .WithName("Name")
                .WithPrompt("Label")
                .WithDefaultValues(default1, default2, default3)
                .Build();

            var defaultValues = _emptyPromptDefaultValueProvider.Get(parameter);

            defaultValues.AssetItemsAndLength(defaultValue1);
        }
    }
}
