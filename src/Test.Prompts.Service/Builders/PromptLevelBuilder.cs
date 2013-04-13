using System.Collections.Generic;
using Prompts.Service.PromptService;
using Prompts.Service.ReportExecution;

namespace Test.Prompts.Service.Builders
{
    class PromptLevelBuilder
    {
        private string _parameterName = "Parameter Name";
        private IEnumerable<ValidValue> _validValues = new[] {new ValidValueBuilder().Build()};
        private bool _hasChildLevel = true;

        public PromptLevelBuilder WithParameterName(string name)
        {
            _parameterName = name;
            return this;
        }

        public PromptLevelBuilder WithAvailableItems(params ValidValue[] availableItems)
        {
            _validValues = availableItems;
            return this;
        }

        public PromptLevelBuilder WithHasChildLevel(bool flag)
        {
            _hasChildLevel = flag;
            return this;
        }

        public PromptLevel Build()
        {
            return new PromptLevel(_parameterName, _validValues, _hasChildLevel);
        }
    }
}
