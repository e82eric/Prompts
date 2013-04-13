using System.Collections.Generic;

namespace Prompts.Service.PromptService.Implementation
{
    public class PromptSelectionsProvider : IPromptSelectionsProvider
    {
        public IPromptSelections Get(IEnumerable<PromptSelectionInfo> enumeration)
        {
            return new PromptSelections(enumeration);
        }
    }
}