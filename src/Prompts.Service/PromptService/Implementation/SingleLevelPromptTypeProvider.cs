namespace Prompts.Service.PromptService.Implementation
{
    public class SingleLevelPromptTypeProvider : IPromptTypeProvider
    {
        public PromptType GetPromptType(SelectionType selectionType)
        {
            switch (selectionType)
            {
                case SelectionType.SingleSelect:
                    return PromptType.DropDown;
                default:
                    return PromptType.ShoppingCart;
            }
        }
    }
}