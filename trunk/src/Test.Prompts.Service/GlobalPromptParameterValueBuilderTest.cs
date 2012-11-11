using NUnit.Framework;
using Prompts.Service.PromptService.Implementation;
using Test.Prompts.Service.Infastructure;

namespace Test.Prompts.Service
{
    [TestFixture]
    public class GlobalPromptParameterValueBuilderTest
    {
        [Test]
        public void ItUsesTheValueParameterNameForPromptName()
        {
            var valueParameter = A.ReportParameter().WithName("Prompt Name").Build();
            var labelParmaeter = A.ReportParameter().Build();

            var builder = new GlobalPromptParameterValueBuilder(valueParameter, labelParmaeter);

            Assert.AreEqual(valueParameter.Name, builder.PromptName);
        }

        [Test]
        public void ParameterValuesAreCreatedForBothTheLabelParmaeterAndValueParmaeter()
        {
            var valueParameter = A.ReportParameter().WithName("Value Parameter").Build();
            var labelParameter = A.ReportParameter().WithName("Label Parameter").Build();

            var selection1 = A.ValidValue().WithValue("selection1").WithLabel("Selection 1").Build();
            var selection2 = A.ValidValue().WithValue("selection2").WithLabel("Selection 2").Build();
            var selections = A.Array(selection1, selection2);

            var prompt = new GlobalPromptParameterValueBuilder(valueParameter, labelParameter);

            var expectedValueParameterValue1 = A.ParameterValue()
                .WithValue(selection1.Value)
                .WithName(valueParameter.Name)
                .Build();

            var expectedLabelParameterValue1 = A.ParameterValue()
                .WithValue(selection1.Label)
                .WithName(labelParameter.Name)
                .Build();

            var expectedValueParameterValue2 = A.ParameterValue()
                .WithValue(selection2.Value)
                .WithName(valueParameter.Name)
                .Build();

            var expectedLabelParameterValue2 = A.ParameterValue()
                .WithValue(selection2.Label)
                .WithName(labelParameter.Name)
                .Build();

            var expectedParameterValues = A.Array(
                expectedValueParameterValue1
                , expectedLabelParameterValue1
                , expectedValueParameterValue2
                , expectedLabelParameterValue2);

            var parameterValues = prompt.BuildParameterValuesFor(selections);

            parameterValues.AssertEqual(expectedParameterValues, (e, a) => e.Name == a.Name && e.Value == a.Value);
        }
    }
}
