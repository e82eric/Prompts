namespace Prompts.Service.PromptService.Implementation
{
    public class HierarchyPromptTypeProvider : IPromptTypeProvider
    {
        public PromptType GetPromptType(SelectionType selectionType)
        {
            switch (selectionType)
            {
                case SelectionType.MultiSelect:
                    return PromptType.Tree;
                default:
                    return PromptType.SingleSelectTree;
            }
        }
    }
}