namespace Prompts.Service.PromptService
{
    public interface IPromptTypeProvider
    {
        PromptType GetPromptType(SelectionType selectionType);
    }
}