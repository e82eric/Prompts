using Prompts.Service.ReportExecution;

namespace Test.Prompts.Service.Builders
{
    public class ParameterValueBuilder
    {
        private string _label = "Label";
        private string _name = "Name";
        private string _value = "Value";

        public ParameterValueBuilder WithLabel(string label)
        {
            _label = label;
            return this;
        }

        public ParameterValueBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public ParameterValueBuilder WithValue(string value)
        {
            _value = value;
            return this;
        }

        public ParameterValue Build()
        {
            return new ParameterValue(){Label = _label, Name = _name, Value = _value};
        }
    }
}
