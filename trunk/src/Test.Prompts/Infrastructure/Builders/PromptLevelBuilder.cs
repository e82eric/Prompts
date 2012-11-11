using System.Collections.Generic;
using Prompts.Service.PromptService;
using Prompts.Service.ReportExecution;

namespace Test.Prompts.Infrastructure.Builders
{
    public class PromptLevelBuilder
    {
        private IEnumerable<ValidValue> _availableItems;
        private bool _hasChildLevel;
        private string _parameterName;

        public PromptLevelBuilder WithAvailableItems(IEnumerable<ValidValue> availableItems)
        {
            _availableItems = availableItems;
            return this;
        }

        public PromptLevelBuilder HasChildLevel(bool hasChildLevel)
        {
            _hasChildLevel = hasChildLevel;
            return this;
        }

        public PromptLevelBuilder WithParameterName(string parameterName)
        {
            _parameterName = parameterName;
            return this;
        }

        public PromptLevel Build()
        {
            return new PromptLevel
                       {
                           AvailableItems = _availableItems
                           , HasChildLevel = _hasChildLevel
                           , ParameterName = _parameterName
                       };
        }
    }
}
