using Prompts.Service.ReportExecution;

namespace Prompts.Service.PromptService
{
    public interface ICasscadingSearchValidator
    {
        void Validate(
            string promptName, 
            ReportParameter searchParameter, 
            ReportParameter resultParameter);
    }
}