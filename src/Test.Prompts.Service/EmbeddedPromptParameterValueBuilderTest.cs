using NUnit.Framework;
using Prompts.Service.PromptService.Implementation;
using Test.Prompts.Service.Infastructure;

namespace Test.Prompts.Service
{
    [TestFixture]
    public class EmbeddedPromptParameterValueBuilderTest
    {
        [Test]
        public void ItUsesTheParametersNameForPromptName()
        {
            var parameter = A.ReportParameter().WithName("Prompt Name").Build();

            var builder = new EmbeddedPromptParameterValueBuilder(parameter);

            Assert.AreEqual(parameter.Name, builder.PromptName);
        }

        [Test]
        public void ParameterValuesAreBuiltWithParameterNameSelectionValueAndSelectionLabel()
        {
            var parameter = A.ReportParameter().WithName("Parmaeter Name").Build();

            var selection1 = A.ValidValue().WithLabel("Selection 1").WithValue("selection1").Build();
            var selection2 = A.ValidValue().WithLabel("Selection 2").WithValue("selection2").Build();
            var selections = A.Array(selection1, selection2);

            var builder = new EmbeddedPromptParameterValueBuilder(parameter);

            var expectedParameterValue1 = A.ParameterValue()
                .WithName(parameter.Name)
                .WithValue(selection1.Value)
                .Build();

            var expectedParameterValue2 = A.ParameterValue()
                .WithName(parameter.Name)
                .WithValue(selection2.Value)
                .Build();

            var expectedParameterValues = A.Array(expectedParameterValue1, expectedParameterValue2);

            var parameterValues = builder.BuildParameterValuesFor(selections);

            parameterValues.AssertEqual(expectedParameterValues, (e, a) => e.Name == a.Name && e.Value == a.Value);
        }
    }
}
