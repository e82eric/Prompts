using System.Collections.Generic;
using Prompts.Service.PromptService;
using Prompts.Service.PromptService.Implementation;
using Prompts.Service.ReportExecution;

namespace Test.Prompts.Service.Builders
{
    class GlobalPromptLevelBuilder
    {
        private string _parameterName = "Parameter Name";
        private IEnumerable<ValidValue> _availableItems = new [] {new ValidValue()};

        public GlobalPromptLevelBuilder WithParameterName(string name)
        {
            _parameterName = name;
            return this;
        }

        public GlobalPromptLevelBuilder WithChildParameterName(string name)
        {
            return this;
        }

        public GlobalPromptLevelBuilder WithAvailableItems(IEnumerable<ValidValue> validValues)
        {
            _availableItems = validValues;
            return this;
        }

        public PromptLevel Build()
        {
            return new PromptLevel(_parameterName, _availableItems, false);
        }
    }
}
