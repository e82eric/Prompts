namespace Prompts.Service.PromptService
{
    public class GlobalPromptBaseReportInfo
    {
        private readonly string _name;
        private readonly string _label;
        private readonly string[] _valueParameterDefaults;
        private readonly string[] _labelParameterDefaults;
        private readonly SelectionType _selectionType;

        public GlobalPromptBaseReportInfo(
            string name,
            string label, 
            string[] valueParameterDefaults, 
            string[] labelParameterDefaults, 
            SelectionType selectionType)
        {
            _selectionType = selectionType;
            _labelParameterDefaults = labelParameterDefaults;
            _valueParameterDefaults = valueParameterDefaults;
            _label = label;
            _name = name;
        }

        public virtual SelectionType SelectionType
        {
            get { return _selectionType; }
        }

        public virtual string[] LabelParameterDefaults
        {
            get { return _labelParameterDefaults; }
        }

        public virtual string[] ValueParameterDefaults
        {
            get { return _valueParameterDefaults; }
        }

        public virtual string Label
        {
            get { return _label; }
        }

        public virtual string Name
        {
            get { return _name; }
        }
    }
}