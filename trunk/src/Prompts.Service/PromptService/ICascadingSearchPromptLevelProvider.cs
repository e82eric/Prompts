using System.Collections.Generic;

namespace Prompts.Service.PromptService
{
    public interface ICascadingSearchPromptLevelProvider
    {
        PromptLevel GetPromptLevel(string searchParameterName, IEnumerable<DefaultValue> defaultValues);
    }
}