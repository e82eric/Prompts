using NUnit.Framework;
using Prompts.Service.PromptService.Implementation;
using Prompts.Service.ReportExecution;
using Test.Prompts.Service.Infastructure;

namespace Test.Prompts.Service
{
    [TestFixture]
    public class HierarchyPromptLevelProviderTest
    {
        private HierarchyPromptLevelProvider _provider;

        [SetUp]
        public void Setup()
        {
            _provider = new HierarchyPromptLevelProvider();
        }

        [Test]
        public void ItReturnsTheCorrectParameterName()
        {
            var parameter = A.ReportParameter().WithName("Parameter Name").Build();

            var promptLevel = _provider.GetPromptLevel(parameter);

            Assert.AreEqual(parameter.Name, promptLevel.ParameterName);
        }

        [Test]
        public void ItReturnsTheParametersAvailableItems()
        {
            var parameter = A.ReportParameter()
                .WithValidValues2( ValidValue("Value 1"), ValidValue("Value 2") )
                .Build();

            var promptLevel = _provider.GetPromptLevel(parameter);

            Assert.AreEqual(parameter.ValidValues, promptLevel.AvailableItems);
        }

        [Test]
        public void ItReturnsTrueForHasChildren()
        {
            var parameter = A.ReportParameter().Build();

            var promptLevel = _provider.GetPromptLevel(parameter);

            Assert.IsTrue(promptLevel.HasChildLevel);
        }

        [Test]
        public void ItReturnsAnEmptyCollectionWhenTheParametersValuesAreNull()
        {
            var parameter = A.ReportParameter().WithValidValues(null).Build();

            var promptLevel = _provider.GetPromptLevel(parameter);

            promptLevel.AvailableItems.AssertLength(0);
        }

        private static ValidValue ValidValue(string value)
        {
            return new ValidValue {Value = value};
        }
    }
}
