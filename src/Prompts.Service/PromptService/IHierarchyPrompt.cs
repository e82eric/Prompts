namespace Prompts.Service.PromptService
{
    public interface IHierarchyPrompt
    {
        PromptLevel GetChildOf(string parameterName);
    }
}