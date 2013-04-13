using Prompts.Service.ReportExecution;

namespace Prompts.Service.PromptService.Implementation
{
    public class RecursiveHierarchyPromptProvider : IHierarchyPromptProvider
    {
        public IHierarchyPrompt Get(ReportParameter[] promptParameters)
        {
            return new RecursiveHierarchyPrompt(promptParameters);
        }
    }
}