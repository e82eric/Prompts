using Prompts.Service.PromptService.Exceptions;

namespace Prompts.Service.PromptService.Implementation
{
    public class CasscadingSearchPromptTypeProvider : IPromptTypeProvider
    {
        public PromptType GetPromptType(SelectionType selectionType)
        {
            switch (selectionType)
            {
                case SelectionType.MultiSelect:
                    return PromptType.CasscadingSearch;
                default:
                    throw new PromptTypeProviderException("A casscading search prompt must be multi-select");
            }
        }
    }
}