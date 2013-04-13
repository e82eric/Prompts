using System.Collections.Generic;

namespace Prompts.Service.PromptService
{
    public class SetPromptSelectionsRequest
    {
        public string Path { get; set; }
        public IEnumerable<PromptSelectionInfo> PromptSelections { get; set; }
    }
}