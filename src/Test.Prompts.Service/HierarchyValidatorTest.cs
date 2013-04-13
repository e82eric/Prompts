using NUnit.Framework;
using Prompts.Service.PromptService.Exceptions;
using Prompts.Service.PromptService.Implementation;
using Test.Prompts.Service.Infastructure;

namespace Test.Prompts.Service
{
    [TestFixture]
    public class HierarchyValidatorTest
    {
        private HierarchyPromptReportValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new HierarchyPromptReportValidator();
        }

        [Test]
        public void ThrowsExceptionWhenDependencyChainIsOutOfOrder()
        {
            const string promptName = "Prompt Name";

            var parameter1 = A.ReportParameter().WithName("Parameter 1").Build();
            var parameter2 = A.ReportParameter().WithName("Parmaeter 2").WithDependency(parameter1.Name).Build();
            var parameter3 = A.ReportParameter().WithName("Parmaeter 3").WithDependency(parameter2.Name).Build();
            var parameters = new[] { parameter1, parameter3, parameter2 };

            var expectedMessage = string.Format
                ("An error occured validating the Tree Prompt '{0}', parameter '{1}' was not dependent on parameter '{2}'"
                , promptName
                , parameter3.Name
                , parameter1.Name);

            ExceptionAssert.Throws<HierarchyValidatorException>
                (expectedMessage 
                , () => _validator.Validate(promptName, parameters));
        }

        [Test]
        public void ThrowsExceptionWhenDependencyIsMissing()
        {
            const string promptName = "Prompt Name";

            var parameter1 = A.ReportParameter().WithName("Parameter 1").Build();
            var parameter2 = A.ReportParameter().WithName("Parmaeter 2").WithDependency(parameter1.Name).Build();
            var parameter3 = A.ReportParameter().WithName("Parmaeter 3").WithDependencies(null).Build();
            var parameters = new[] { parameter1, parameter2, parameter3 };

            var expectedMessage = string.Format
                ("An error occured validating the Tree Prompt '{0}', parameter '{1}' was not dependent on parameter '{2}'"
                , promptName
                , parameter3.Name
                , parameter2.Name);

            ExceptionAssert.Throws<HierarchyValidatorException>
                (expectedMessage
                , () => _validator.Validate(promptName, parameters));
        }

        [Test]
        public void ThrowsExceptionWhenAParameterHasMoreThanOneDependency()
        {
            const string promptName = "Prompt Name";

            var parameter1 = A.ReportParameter().WithName("Parameter 1").Build();
            var parameter2 = A.ReportParameter().WithName("Parameter 2").WithDependency(parameter1.Name).Build();
            var parameter3 =
                A.ReportParameter().WithName("Parameter 3").WithDependencies(new[] { parameter1.Name, parameter2.Name }).
                    Build();
            var parameters = new[] { parameter1, parameter2, parameter3 };

            var expectedMessage = string.Format
                ("An error occured validating the Tree Prompt '{0}', parameter '{1}' has more than one dependency"
                , promptName
                , parameter3.Name);

            ExceptionAssert.Throws<HierarchyValidatorException>
                (expectedMessage
                , () => _validator.Validate(promptName, parameters));
        }
    }
}
