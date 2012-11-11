using Prompts.Service.PromptService.Exceptions;
using Prompts.Service.ReportExecution;

namespace Prompts.Service.PromptService.Implementation
{
    public class CasscadingSearchValidator : ICasscadingSearchValidator
    {
        public void Validate(string promptName, ReportParameter searchParameter, ReportParameter resultParameter)
        {
            if (searchParameter.ValidValues != null)
            {
                throw new PromptInfoProviderException(
                    string.Format(
                        "Error building Search Prompt Report '{0}', first parameters valid values were not null",
                        promptName));
            }

            if (resultParameter.Dependencies == null
                || resultParameter.Dependencies[0] != searchParameter.Name
                || resultParameter.Dependencies.Length > 1)
            {
                throw new PromptInfoProviderException(
                    string.Format(
                    "Error building Search Prompt Report '{0}', the result parameter must be dependent on the search parameter and have only one dependency", 
                    promptName));
            }
        }
    }
}