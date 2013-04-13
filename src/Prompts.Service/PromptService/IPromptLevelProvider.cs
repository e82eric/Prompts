using Prompts.Service.ReportExecution;

namespace Prompts.Service.PromptService
{
    public interface IPromptLevelProvider
    {
        PromptLevel GetPromptLevel(ReportParameter reportParameter);
    }
}