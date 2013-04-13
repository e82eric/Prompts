using System.Collections.ObjectModel;
using Prompts.Service.PromptService;

namespace Test.Prompts.Infrastructure.Builders
{
    public class PromptInfoBuilder
    {
        private string _name = "Prompt Name";
        private string _label = "Label";
        private PromptLevel _promptLevel = A.PromptLevel().Build();
        private ObservableCollection<DefaultValue> _defaultValues = new ObservableCollection<DefaultValue>();

        public PromptInfoBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public PromptInfoBuilder WithPromptLevel(PromptLevel promptLevel)
        {
            _promptLevel = promptLevel;
            return this;
        }

        public PromptInfoBuilder WithDefaultValues(ObservableCollection<DefaultValue> defaultValues)
        {
            _defaultValues = defaultValues;
            return this;
        }

        public PromptInfoBuilder WithLabel(string label)
        {
            _label = label;
            return this;
        }

        public PromptInfo Build()
        {
            return new PromptInfo
                       {
                           DefaultValues = _defaultValues,
                           Label = _label,
                           Name = _name,
                           PromptLevelInfo = _promptLevel,
                           PromptType = PromptType.Tree
                       };
        }


    }
}
