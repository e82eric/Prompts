using Prompts.Service.PromptService;
using Prompts.Service.ReportExecution;

namespace Prompts.Prompting.ViewModels.Implementation
{
    public class EmptyPrompt : Prompt
    {
        private string _text;

        public EmptyPrompt(string name, string label) 
            : base(name, label)
        {
        }
        
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                SetReadyForReportExecutionChangedIfNeeded();
                RaisePropertyChanged("Text");
            }
        }

        public override PromptSelectionInfo ToSelectionInfo()
        {
            return new PromptSelectionInfo
                {
                    PromptName = Name,
                    Selections = new[] {new ValidValue {Value = Text}}
                };
        }

        protected override bool EvaluateReadyForReportExecution()
        {
            return string.IsNullOrEmpty(Text) ? false : true;
        }
    }
}
