namespace Prompts.Service.PromptService
{
    public class DefaultValue
    {
        private readonly string _value;
        private readonly bool _isAllMember;
        private readonly string _label;

        public DefaultValue(string value, string label, bool isAllMember)
        {
            _label = label;
            _isAllMember = isAllMember;
            _value = value;
        }

        public string Label
        {
            get { return _label; }
        }

        public bool IsAllMember
        {
            get { return _isAllMember; }
        }

        public string Value
        {
            get { return _value; }
        }
    }
}