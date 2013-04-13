using System.Collections.Generic;
using Prompts.Service.PromptService;
using Prompts.Service.ReportExecution;

namespace Test.Prompts.Service.Builders
{
    class PromptSelectionInfoBuilder
    {
        private string _promptName;
        private IEnumerable<ValidValue> _selections;

        public PromptSelectionInfoBuilder WithPromptName(string name)
        {
            _promptName = name;
            return this;
        }

        public PromptSelectionInfoBuilder WithSelections(IEnumerable<ValidValue> selections)
        {
            _selections = selections;
            return this;
        }

        public PromptSelectionInfo Build()
        {
            return new PromptSelectionInfo(_promptName, _selections);
        }
    }
}
