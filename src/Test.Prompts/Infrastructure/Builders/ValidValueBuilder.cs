using Prompts.Service.ReportExecution;

namespace Test.Prompts.Infrastructure.Builders
{
    public class ValidValueBuilder
    {
        private string _value = "Value";
        private string _label = "Label";

        public ValidValueBuilder WithValue(string value)
        {
            _value = value;
            return this;
        }

        public ValidValueBuilder WithLabel(string label)
        {
            _label = label;
            return this;
        }

        public ValidValue Build()
        {
            return new ValidValue {Value = _value, Label = _label};
        }
    }
}
