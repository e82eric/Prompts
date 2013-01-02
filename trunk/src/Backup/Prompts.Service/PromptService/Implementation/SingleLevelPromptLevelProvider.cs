using Prompts.Service.ReportExecution;

namespace Prompts.Service.PromptService.Implementation
{
    public class SingleLevelPromptLevelProvider : IPromptLevelProvider
    {
        public PromptLevel GetPromptLevel(ReportParameter reportParameter)
        {
            return new PromptLevel(reportParameter.Name, reportParameter.ValidValues?? new ValidValue[]{}, false);
        }
    }
}