using Prompts.Service.ReportExecution;

namespace Prompts.Service.PromptService
{
    public interface IHierarchyPromptProvider
    {
        IHierarchyPrompt Get(ReportParameter[] promptParameters);
    }
}