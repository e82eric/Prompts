using Prompts.Service.PromptService;

namespace Test.Prompts.Infrastructure.Builders
{
    public class DefaultValueBuilder
    {
        private string _value = "Value";
        private string _label = "Label";
        private bool _isAllMember = false;

        public DefaultValueBuilder WithValue(string value)
        {
            _value = value;
            return this;
        }

        public DefaultValueBuilder WithLabel(string label)
        {
            _label = label;
            return this;
        }

        public DefaultValueBuilder WithIsAllMember(bool flag)
        {
            _isAllMember = flag;
            return this;
        }

        public DefaultValue Build()
        {
            return new DefaultValue{Value = _value, Label = _label, IsAllMember = _isAllMember};
        }
    }
}