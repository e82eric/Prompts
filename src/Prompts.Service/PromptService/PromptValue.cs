using System.Collections.Generic;

namespace Prompts.Service.PromptService
{
    public class PromptValue
    {
        public string Label { get; set; }
        public string Value { get; set; }
        public IEnumerable<PromptValue> Children { get; set; }
    }
}