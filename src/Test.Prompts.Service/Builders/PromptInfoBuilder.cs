using System.Collections.Generic;
using Prompts.Service.PromptService;

namespace Test.Prompts.Service.Builders
{
    class PromptInfoBuilder
    {
        private string _name;
        private string _label;
        private PromptType _promptType;
        private PromptLevel _promptLevel;
        private IEnumerable<DefaultValue> _defaultValues;

        public PromptInfoBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public PromptInfoBuilder WithLabel(string label)
        {
            _label = label;
            return this;
        }

        public PromptInfoBuilder WithPromptType(PromptType promptType)
        {
            _promptType = promptType;
            return this;
        }

        public PromptInfoBuilder WithPromptLevel(PromptLevel promptLevel)
        {
            _promptLevel = promptLevel;
            return this;
        }

        public PromptInfoBuilder WithDefaultValues(IEnumerable<DefaultValue> defaultValues)
        {
            _defaultValues = defaultValues;
            return this;
        }

        public PromptInfo Build()
        {
            return new PromptInfo(_name, _label, _promptType, _promptLevel, _defaultValues);
        }
    }
}
