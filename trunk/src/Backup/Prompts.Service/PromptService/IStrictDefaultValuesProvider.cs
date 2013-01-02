using System.Collections.Generic;

namespace Prompts.Service.PromptService
{
    public interface IStrictDefaultValuesProvider
    {
        IEnumerable<DefaultValue> GetDefaultValues(PromptLevel promptLevel, IEnumerable<string> defaultValues);
    }
}