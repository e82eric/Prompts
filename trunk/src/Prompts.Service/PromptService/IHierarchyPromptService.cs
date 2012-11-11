using Prompts.Service.ReportExecution;

namespace Prompts.Service.PromptService
{
    public interface IHierarchyPromptService
    {
        IHierarchyPrompt GetHierarchyPrompt(string promptName, ParameterValue[] parameterValues);
    }
}