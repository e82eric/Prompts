using NUnit.Framework;
using Prompts.Service.PromptService;
using Prompts.Service.PromptService.Implementation;
using System.Linq;
using Test.Prompts.Service.Infastructure;

namespace Test.Prompts.Service
{
    [TestFixture]
    public class CasscadingPromptLevelProviderTest
    {
        private CascadingSearchPromptLevelProvider _provider;

        [SetUp]
        public void Setup()
        {
            _provider = new CascadingSearchPromptLevelProvider();
        }

        [Test]
        public void ItReturnsTheCorrectParameterName()
        {
            const string searchParameterName = "Parameter Name";

            var promptLevel = _provider.GetPromptLevel(searchParameterName, new DefaultValue[]{});

            Assert.AreEqual(searchParameterName, promptLevel.ParameterName);
        }

        [Test]
        public void ItUsesTheDefaultValuesForAvailableItems()
        {
            var defaultValues = A.Array(A.DefaultValue().Build());

            var promptLevel = _provider.GetPromptLevel("Parameter Name", defaultValues);

            Assert.AreEqual(defaultValues.AssertSingle().Value, promptLevel.AvailableItems.AssertSingle().Value);
            Assert.AreEqual(defaultValues.AssertSingle().Label, promptLevel.AvailableItems.AssertSingle().Label);
        }

        [Test]
        public void ItUsesTheDefaultValuesForAvailableItemsWhenThereAreMultipleDefaultValues()
        {
            var defaultValue1 = A.DefaultValue().WithValue("value1").WithLabel("label1").Build();
            var defaultValue2 = A.DefaultValue().WithValue("value2").WithLabel("label2").Build();
            var defaultValue3 = A.DefaultValue().WithValue("value3").WithLabel("label3").Build();
            var defaultValues = A.Array(defaultValue1, defaultValue2, defaultValue3);

            var promptLevel = _provider.GetPromptLevel("Parameter Name", defaultValues);

            promptLevel.AvailableItems.AssertLength(3);

            Assert.IsNotNull(promptLevel.AvailableItems.Where(i => i.Value == defaultValue1.Value && i.Label == defaultValue1.Label));
            Assert.IsNotNull(promptLevel.AvailableItems.Where(i => i.Value == defaultValue2.Value && i.Label == defaultValue2.Label));
            Assert.IsNotNull(promptLevel.AvailableItems.Where(i => i.Value == defaultValue3.Value && i.Label == defaultValue3.Label));
        }

        [Test]
        public void ItReturnsTrueForHasChildLevel()
        {
            const string searchParameterName = "Parameter Name";

            var promptLevel = _provider.GetPromptLevel(searchParameterName, new DefaultValue[] { });

            Assert.IsTrue(promptLevel.HasChildLevel);
        }
    }
}
