using NUnit.Framework;
using Prompts.Service.PromptService.Exceptions;
using Prompts.Service.PromptService.Implementation;
using Test.Prompts.Service.Infastructure;

namespace Test.Prompts.Service
{
    [TestFixture]
    public class CasscadingSearchValidatorTest
    {
        private CasscadingSearchValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new CasscadingSearchValidator();
        }

        [Test]
        public void ThrowsExceptionWhenTheFirstParametersValidValuesAreNotNullForCasscadingSearch()
        {
            const string promptName = "Prompt Name";

            var badSearchParameter = A.ReportParameter().WithValidValues(A.ValidValue("Value 1")).Build();
            var resultParameter = A.ReportParameter().WithDependency(badSearchParameter.Name).Build();

            var expectedExceptionMessage = string.Format(
                "Error building Search Prompt Report '{0}', first parameters valid values were not null"
                , promptName);

            ExceptionAssert.Throws<PromptInfoProviderException>
                (expectedExceptionMessage
                , () => _validator.Validate(promptName, badSearchParameter, resultParameter));
        }

        [Test]
        public void ItThrowsAnExceptionWhenTheSecondParameterHasNoDependencies()
        {
            const string promptName = "Prompt Name";

            var searchParameter = A.ReportParameter().WithValidValues(null).Build();
            var resultParameter = A.ReportParameter().WithDependencies(null).Build();

            var expectedExceptionMessage = string.Format(
                "Error building Search Prompt Report '{0}', the result parameter must be dependent on the search parameter and have only one dependency"
                , promptName);

            ExceptionAssert.Throws<PromptInfoProviderException>
                (expectedExceptionMessage
                , () => _validator.Validate(promptName, searchParameter, resultParameter));
        }

        [Test]
        public void ThrowsExceptionWhenTheSecondParameterHasADependencyOtherThanTheSearchParameter()
        {
            const string promptName = "Prompt Name";

            var searchParameter = A.ReportParameter().WithValidValues(null).Build();
            var resultParameter = A.ReportParameter().WithDependencies("Not Search Parameter").Build();

            var expectedExceptionMessage = string.Format(
                "Error building Search Prompt Report '{0}', the result parameter must be dependent on the search parameter and have only one dependency"
                , promptName);

            ExceptionAssert.Throws<PromptInfoProviderException>
                (expectedExceptionMessage
                , () => _validator.Validate(promptName, searchParameter, resultParameter));
        }

        [Test]
        public void ItThrowsAnExceptionWhenTheSecondParameterHasMulipleDependencies()
        {
            const string promptName = "Prompt Name";

            var searchParameter = A.ReportParameter().WithValidValues(null).Build();
            var resultParameter = A.ReportParameter().WithDependencies("Not Search Parameter", "Not Search Parameter 2").Build();

            var expectedExceptionMessage = string.Format(
                "Error building Search Prompt Report '{0}', the result parameter must be dependent on the search parameter and have only one dependency"
                , promptName);

            ExceptionAssert.Throws<PromptInfoProviderException>
                (expectedExceptionMessage
                , () => _validator.Validate(promptName, searchParameter, resultParameter));
        }
    }
}
