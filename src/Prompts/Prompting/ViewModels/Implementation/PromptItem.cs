using Prompts.Infastructure;
using Prompts.Service.ReportExecution;

namespace Prompts.Prompting.ViewModels.Implementation
{
    public class PromptItem : ViewModelBase, IPromptItem
    {
        private readonly string _promptName;
        private readonly string _parameterName;
        private readonly ValidValue _validValue;
        private bool _isDefaultAll;

        public PromptItem(string promptName, string parameterName, ValidValue validValue, bool isDefaultAll)
        {
            _isDefaultAll = isDefaultAll;
            _promptName = promptName;
            _parameterName = parameterName;
            _validValue = validValue;
        }

        public bool IsDefaultAll
        {
            get { return _isDefaultAll; }
            set { _isDefaultAll = value; }
        }

        public string Label
        {
            get { return _validValue.Label; }
        }

        public string Value
        {
            get { return _validValue.Value; }
        }

        public string PromptName
        {
            get { return _promptName; }
        }

        public string ParameterName
        {
            get { return _parameterName; }
        }
    }
}