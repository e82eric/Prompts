namespace Prompts.Service.PromptService.Implementation
{
    public class RecursiveHierarchyPromptTypeProvider : IPromptTypeProvider
    {
        public PromptType GetPromptType(SelectionType selectionType)
        {
            switch (selectionType)
            {
                case SelectionType.MultiSelect:
                    return PromptType.RecursiveTree;
                default:
                    return PromptType.RecursiveSingleSelectTree;
            }
        }
    }
}