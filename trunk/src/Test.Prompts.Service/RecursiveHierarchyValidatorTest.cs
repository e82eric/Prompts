using NUnit.Framework;
using Prompts.Service.PromptService.Exceptions;
using Prompts.Service.PromptService.Implementation;
using Prompts.Service.ReportExecution;
using Test.Prompts.Service.Infastructure;

namespace Test.Prompts.Service
{
    [TestFixture]
    public class RecursiveHierarchyValidatorTest
    {
        private RecursiveHierarchyValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new RecursiveHierarchyValidator();
        }

        [Test]
        public void ItThrowsAnExceptionWhenTheParametersAreNull()
        {
            const string promptName = "Prompt Name";

            var expectedMessage = string.Format(
                "An error occured validating the Recursive Tree Prompt '{0}': there were not exactly 2 parameters",
                promptName);

            ExceptionAssert.Throws<HierarchyValidatorException>
                (expectedMessage
                , () => _validator.Validate(promptName, null));
        }

        [Test]
        public void ItThrowsAnExceptionWhenThereAreNoParameters()
        {
            const string promptName = "Prompt Name";

            var expectedMessage = string.Format(
                "An error occured validating the Recursive Tree Prompt '{0}': there were not exactly 2 parameters",
                promptName);

            ExceptionAssert.Throws<HierarchyValidatorException>
                (expectedMessage
                , () => _validator.Validate(promptName, new ReportParameter[]{}));
        }

        [Test]
        public void ItThrowsAnExceptionWhenThereAreFewerThan2Parameters()
        {
            const string promptName = "Prompt Name";

            var parameter1 = A.ReportParameter().WithName("Parameter 1").Build();

            var expectedMessage = string.Format(
                "An error occured validating the Recursive Tree Prompt '{0}': there were not exactly 2 parameters",
                promptName);

            ExceptionAssert.Throws<HierarchyValidatorException>
                (expectedMessage
                , () => _validator.Validate(promptName, A.Array(parameter1)));
        }

        [Test]
        public void ItThrowsAnExceptionWhenThereAreMoreThan2Parameters()
        {
            const string promptName = "Prompt Name";

            var parameter1 = A.ReportParameter().WithName("Parameter 1").Build();
            var parameter2 = A.ReportParameter().WithName("Parameter 2").Build();
            var parameter3 = A.ReportParameter().WithName("Parameter 3").Build();

            var expectedMessage = string.Format(
                "An error occured validating the Recursive Tree Prompt '{0}': there were not exactly 2 parameters",
                promptName);

            ExceptionAssert.Throws<HierarchyValidatorException>
                (expectedMessage
                , () => _validator.Validate(promptName, A.Array(parameter1, parameter2, parameter3)));
        }

        [Test]
        public void ItThrowsAnExceptionWhenTheFirstParametersValidValuesAreNotNull()
        {
            const string promptName = "Prompt Name";

            var parameter1 = A.ReportParameter().WithName("Parameter 1").WithValidValues(new ValidValue()).Build();
            var parameter2 = A.ReportParameter().WithName("Parameter 2").WithDependencies(parameter1.Name).Build();

            var expectedMessage = string.Format(
                "An error occured validating the Recursive Tree Prompt '{0}': first parameters valid values were not null",
                promptName);

            ExceptionAssert.Throws<HierarchyValidatorException>
                (expectedMessage
                , () => _validator.Validate(promptName, A.Array(parameter1, parameter2)));
        }

        [Test]
        public void ItThrowsAnExceptionWhenTheSecondParameterIsNotDependentOnTheFirstParameter()
        {
            const string promptName = "Prompt Name";

            var parameter1 = A.ReportParameter().WithName("Parameter 1").WithValidValues(null).Build();
            var parameter2 = A.ReportParameter().WithName("Parameter 2").Build();

            var expectedMessage = string.Format(
                "An error occured validating the Recursive Tree Prompt '{0}': the result parameter must be dependent on the filter parameter",
                promptName);

            ExceptionAssert.Throws<HierarchyValidatorException>
                (expectedMessage
                , () => _validator.Validate(promptName, A.Array(parameter1, parameter2)));
        }
    }
}
