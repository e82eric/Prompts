using System.Collections.Generic;

namespace Prompts.Service.PromptService
{
    public interface IPromptSelectionsProvider
    {
        IPromptSelections Get(IEnumerable<PromptSelectionInfo> enumeration);
    }
}