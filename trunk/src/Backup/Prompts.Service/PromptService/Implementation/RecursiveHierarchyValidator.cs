using System.Linq;
using Prompts.Service.PromptService.Exceptions;
using Prompts.Service.ReportExecution;

namespace Prompts.Service.PromptService.Implementation
{
    public class RecursiveHierarchyValidator : IHierarchyPromptReportValidator
    {
        public void Validate(string promptName, ReportParameter[] promptReportParameters)
        {
            if(promptReportParameters == null)
            {
                throw GetExactly2ParametersException(promptName);
            }
            if(promptReportParameters.Length != 2)
            {
                throw GetExactly2ParametersException(promptName);
            }
            if (promptReportParameters[0].ValidValues != null)
            {
                throw GetException(promptName, "first parameters valid values were not null");
            }
            if(promptReportParameters[1].Dependencies.Single() != promptReportParameters[0].Name)
            {
                throw GetException(
                    promptName,
                    "the result parameter must be dependent on the filter parameter");
            }
        }

        private static HierarchyValidatorException GetExactly2ParametersException(string promptName)
        {
            return GetException(promptName, "there were not exactly 2 parameters");
        }

        private static HierarchyValidatorException GetException(string promptName, string endOfErrorMessage)
        {
            var errorMessage = string.Format(
                "An error occured validating the Recursive Tree Prompt '{0}': {1}",
                promptName,
                endOfErrorMessage);

            return new HierarchyValidatorException(errorMessage);
        }
    }
}