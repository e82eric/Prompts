using Prompts.Service.PromptService;
using Test.Prompts.Service.Infastructure;

namespace Test.Prompts.Service.Builders
{
    public class GlobalPromptBaseReportInfoBuilder
    {
        private string _name = "Name";
        private string _label = "Label";
        private string[] _valueParameterDefaults = A.Array("Default 1", "Default 2");
        private string[] _labelParameterDefaults = A.Array("Default 1");
        private SelectionType _selectionType = SelectionType.SingleSelect;

        public GlobalPromptBaseReportInfoBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public GlobalPromptBaseReportInfoBuilder WithLabel(string label)
        {
            _label = label;
            return this;
        }

        public GlobalPromptBaseReportInfoBuilder WithValueParameterDefaults(params string[] defaults)
        {
            _valueParameterDefaults = defaults;
            return this;
        }

        public GlobalPromptBaseReportInfoBuilder WithLabelParameterDefaults(params string[] defaults)
        {
            _labelParameterDefaults = defaults;
            return this;
        }

        public GlobalPromptBaseReportInfoBuilder WithSelectionType(SelectionType selectionType)
        {
            _selectionType = selectionType;
            return this;
        }

        public GlobalPromptBaseReportInfo Build()
        {
            return new GlobalPromptBaseReportInfo(
                _name,
                _label, 
                _valueParameterDefaults, 
                _labelParameterDefaults, 
                _selectionType);
        }
    }
}
