using Moq;
using NUnit.Framework;
using Prompts.Service.PromptService;
using Prompts.Service.PromptService.Implementation;
using Test.Prompts.Service.Infastructure;

namespace Test.Prompts.Service
{
    [TestFixture]
    public class PromptSelectionCollectionTest
    {
        [Test]
        public void ItSendsTheCorrectParameterValuesToThePromptBaseOnName2()
        {
            const string promptName = "Prompt 2";

            var validValues = A.Array(A.ValidValue().Build(), A.ValidValue().Build());

            var parameterValues = A.Array(A.ParameterValue().Build(), A.ParameterValue().Build());

            var promptSelection1 = A.PromptSelectionInfo().WithPromptName("Prompt 1").Build();
            var promptSelection2 = A.PromptSelectionInfo()
                .WithPromptName(promptName)
                .WithSelections(validValues)
                .Build();

            var promptSelections = A.Array(promptSelection1, promptSelection2);

            var parameterValueBuilder =
                Mock.Of<IParameterValueBuilder>(
                    b =>
                    b.BuildParameterValuesFor(validValues) == parameterValues &&
                    b.PromptName == promptName);

            var collection = new PromptSelections(promptSelections);

            var returnedParameterValues = collection.CreateParameterValuesFor(parameterValueBuilder);

            Assert.AreEqual(parameterValues, returnedParameterValues);
        }

        [Test]
        public void ItOnlyUsesTheSelectionsWithTheSameNameAsThePrompt()
        {
            const string promptName = "Prompt 2";

            var validValues = A.Array(A.ValidValue().Build(), A.ValidValue().Build());

            var parameterValues = A.Array(A.ParameterValue().Build(), A.ParameterValue().Build());

            var promptSelection1 = A.PromptSelectionInfo().WithPromptName("Prompt 1").Build();
            var promptSelection2 = A.PromptSelectionInfo()
                .WithPromptName(promptName)
                .WithSelections(validValues)
                .Build();

            var promptSelections = A.Array(promptSelection1, promptSelection2);

            var parameterValueBuilder =
                Mock.Of<IParameterValueBuilder>(
                    b =>
                    b.BuildParameterValuesFor(validValues) == parameterValues &&
                    b.PromptName == promptName);

            var collection = new PromptSelections(promptSelections);

            var returnedParameterValues = collection.CreateParameterValuesFor(parameterValueBuilder);

            Assert.AreEqual(parameterValues, returnedParameterValues);
        }
    }
}