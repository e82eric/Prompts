using Moq;
using NUnit.Framework;
using Prompts.Service.PromptService.Implementation;
using Prompts.Service.PromptService;
using Test.Prompts.Service.Infastructure;

namespace Test.Prompts.Service
{
    [TestFixture]
    public class StrictDefaultValuesProviderTest
    {
        private StrictDefaultValuesProvider _provider;
        private Mock<IDefaultValueProvider> _defaultValueProvider;

        [SetUp]
        public void Setup()
        {
            _defaultValueProvider = new Mock<IDefaultValueProvider>();
            _provider = new StrictDefaultValuesProvider(_defaultValueProvider.Object);
        }

        [Test]
        public void ItReturnsAnEmptyCollectionWhenThereAreNoDefaults()
        {
            var promptLevel = A.PromptLevel()
                .WithAvailableItems(A.ValidValue("Value 1"), A.ValidValue("Value 2"))
                .Build();

            var defautls = new string[] { };

            var defaultValues = _provider.GetDefaultValues(promptLevel, defautls);

            defaultValues.AssertLength(0);
        }

        [Test]
        public void ItReturnsAnEmptyCollectionWhenThereAreAvailableItemsThatHaveTheSameValueAsTheDefault()
        {
            var promptLevel = A.PromptLevel()
                .WithAvailableItems(A.ValidValue("Value 1"), A.ValidValue("Value 2"), A.ValidValue("Value 3"))
                .Build();

            const string defaultString1 = "String1";
            const string defaultString2 = "String2";
            const string defaultString3 = "String3";

            var defaultValue1 = A.DefaultValue("Default1");
            var defaultValue2 = A.DefaultValue("Default2");
            var defaultValue3 = A.DefaultValue("Default3");

            _defaultValueProvider.Setup(p => p.Get(defaultString1, string.Empty)).Returns(defaultValue1);
            _defaultValueProvider.Setup(p => p.Get(defaultString2, string.Empty)).Returns(defaultValue2);
            _defaultValueProvider.Setup(p => p.Get(defaultString3, string.Empty)).Returns(defaultValue3);

            var defaults = A.Array(defaultString1, defaultString2, defaultString3);

            var defaultValues = _provider.GetDefaultValues(promptLevel, defaults);

            defaultValues.AssertLength(0);
        }

        [Test]
        public void ItOnlyReturnsValidValuesForDefautlsThatAvaialbleItemsExistInThe()
        {
            const string value1 = "Value 1";
            const string value2 = "Value 2";
            const string value3 = "Value 3";

            const string defaultString1 = "String1";
            const string defaultString2 = "String2";
            const string defaultString3 = "String3";

            var defaultValue1 = A.DefaultValue("Default1");
            var defaultValue2 = A.DefaultValue("Value 2");
            var defaultValue3 = A.DefaultValue("Value 3");

            _defaultValueProvider.Setup(p => p.Get(defaultString1, string.Empty)).Returns(defaultValue1);
            _defaultValueProvider.Setup(p => p.Get(defaultString2, string.Empty)).Returns(defaultValue2);
            _defaultValueProvider.Setup(p => p.Get(defaultString3, string.Empty)).Returns(defaultValue3);

            var promptLevel = A.PromptLevel()
                .WithAvailableItems(A.ValidValue(value1), A.ValidValue(value2), A.ValidValue(value3))
                .Build();

            var defaults = A.Array(defaultString1, defaultString2, defaultString3);

            var defaultValues = _provider.GetDefaultValues(promptLevel, defaults);

            defaultValues.AssetItemsAndLength(defaultValue2, defaultValue3);
        }

        [Test]
        public void ItReturnsTheAvailableItemsThatHaveTheSameValueAsTheValueParameterDefaults()
        {
            const string value1 = "Value 1";
            const string value2 = "Value 2";
            const string value3 = "Value 3";

            const string defaultString1 = "String1";
            const string defaultString2 = "String2";
            const string defaultString3 = "String3";

            var defaultValue1 = A.DefaultValue("Value 1");
            var defaultValue2 = A.DefaultValue("Default2");
            var defaultValue3 = A.DefaultValue("Value 3");

            _defaultValueProvider.Setup(p => p.Get(defaultString1, string.Empty)).Returns(defaultValue1);
            _defaultValueProvider.Setup(p => p.Get(defaultString2, string.Empty)).Returns(defaultValue2);
            _defaultValueProvider.Setup(p => p.Get(defaultString3, string.Empty)).Returns(defaultValue3);

            var promptLevel = A.PromptLevel()
                .WithAvailableItems(A.ValidValue(value1), A.ValidValue(value2), A.ValidValue(value3))
                .Build();

            var defaults = A.Array(defaultString1, defaultString2, defaultString3);

            var defaultValues = _provider.GetDefaultValues(promptLevel, defaults);

            defaultValues.AssetItemsAndLength(defaultValue1, defaultValue3);
        }
    }
}
