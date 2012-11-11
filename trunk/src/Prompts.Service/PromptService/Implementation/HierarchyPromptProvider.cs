using Prompts.Service.ReportExecution;

namespace Prompts.Service.PromptService.Implementation
{
    public class HierarchyPromptProvider : IHierarchyPromptProvider
    {
        public IHierarchyPrompt Get(ReportParameter[] promptParameters)
        {
            return new HierarchyPrompt(promptParameters);
        }
    }
}