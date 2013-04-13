using Prompts.Service.ReportExecution;

namespace Prompts.Service.PromptService.Implementation
{
    public class HierarchyPromptLevelProvider : IPromptLevelProvider
    {
        public PromptLevel GetPromptLevel(ReportParameter reportParameter)
        {
            return new PromptLevel(reportParameter.Name, reportParameter.ValidValues ?? new ValidValue[]{}, true);
        }
    }
}