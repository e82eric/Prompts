using NUnit.Framework;
using Prompts.Service.PromptService.Implementation;
using Test.Prompts.Service.Infastructure;

namespace Test.Prompts.Service
{
    [TestFixture]
    public class SingleLevelPromptLevelProviderTest
    {
        private SingleLevelPromptLevelProvider _provider;

        [SetUp]
        public void Setup()
        {
            _provider = new SingleLevelPromptLevelProvider();
        }

        [Test]
        public void ItReturnsTheParametersAvailableItemsForSingleLevel()
        {
            var parameter = A.ReportParameter()
                .WithValidValues2(A.ValidValue("Value 1"), A.ValidValue("Value 2"))
                .Build();

            var promptLevel = _provider.GetPromptLevel(parameter);

            Assert.AreEqual(parameter.ValidValues, promptLevel.AvailableItems);
        }

        [Test]
        public void ItReturnsFalseForHasChildrenForSingleLevel()
        {
            var parameter = A.ReportParameter().Build();

            var promptLevel = _provider.GetPromptLevel(parameter);

            Assert.IsFalse(promptLevel.HasChildLevel);
        }

        [Test]
        public void ItReturnsAnEmptyCollectionWhenTheParametersValuesAreNullForSingleLevel()
        {
            var parameter = A.ReportParameter().WithValidValues(null).Build();

            var promptLevel = _provider.GetPromptLevel(parameter);

            promptLevel.AvailableItems.AssertLength(0);
        }

        [Test]
        public void ItReturnsTheCorrectParameterNameForSingleLevel()
        {
            var parameter = A.ReportParameter().WithName("Parameter Name").Build();

            var promptLevel = _provider.GetPromptLevel(parameter);

            Assert.AreEqual(parameter.Name, promptLevel.ParameterName);
        }

        [Test]
        public void ItReturnsTheParametersAvailableItemsForHierarchy()
        {
            var parameter = A.ReportParameter()
                .WithValidValues2(A.ValidValue("Value 1"), A.ValidValue("Value 2"))
                .Build();

            var promptLevel = _provider.GetPromptLevel(parameter);

            Assert.AreEqual(parameter.ValidValues, promptLevel.AvailableItems);
        }

        [Test]
        public void ItReturnsTrueForHasChildrenForHierarchy()
        {
            var parameter = A.ReportParameter().Build();

            var promptLevel = _provider.GetPromptLevel(parameter);

            Assert.IsFalse(promptLevel.HasChildLevel);
        }

        [Test]
        public void ItReturnsAnEmptyCollectionWhenTheParametersValuesAreNullForHierarchy()
        {
            var parameter = A.ReportParameter().WithValidValues(null).Build();

            var promptLevel = _provider.GetPromptLevel(parameter);

            promptLevel.AvailableItems.AssertLength(0);
        }
    }
}
