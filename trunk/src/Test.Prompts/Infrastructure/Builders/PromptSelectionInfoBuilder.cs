using System.Collections.ObjectModel;
using Prompts.Service.PromptService;
using Prompts.Service.ReportExecution;

namespace Test.Prompts.Infrastructure.Builders
{
    public class PromptSelectionInfoBuilder
    {
        private string _promptName = "Prompt Name";
        private ObservableCollection<ValidValue> _selections = new ObservableCollection<ValidValue>();

        public PromptSelectionInfoBuilder WithPromptName(string name)
        {
            _promptName = name;
            return this;
        }

        public PromptSelectionInfoBuilder WithSelections(ObservableCollection<ValidValue> selections)
        {
            _selections = selections;
            return this;
        }

        public PromptSelectionInfo Build()
        {
            return new PromptSelectionInfo {PromptName = _promptName, Selections = _selections};
        }
    }
}
