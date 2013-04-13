using System.Collections.Generic;
using System.Linq;
using Prompts.Service.ReportExecution;

namespace Prompts.Service.PromptService.Implementation
{
    public class CascadingSearchPromptLevelProvider : ICascadingSearchPromptLevelProvider
    {
        public PromptLevel GetPromptLevel(string searchParameterName, IEnumerable<DefaultValue> defaultValues)
        {
            var availableItems = defaultValues.Select(v => new ValidValue {Value = v.Value, Label = v.Label}).ToList();

            return new PromptLevel(searchParameterName, availableItems, true);
        }
    }
}