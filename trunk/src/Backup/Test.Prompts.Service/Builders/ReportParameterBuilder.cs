using Prompts.Service.ReportExecution;

namespace Test.Prompts.Service.Builders
{
    public class ReportParameterBuilder
    {
        private string _name = "Name";
        private bool _multiValueFlag = true;
        private ValidValue[] _validValues = new[] {new ValidValue()};
        private string[] _dependencies = new[]{"Dependent"};
        private string _prompt = "Prompt";
        private string[] _defaultValue = new string[]{};
        private bool _promptUser = true;

        public ReportParameterBuilder WithDependencies(params string[] dependencies)
        {
            _dependencies = dependencies;
            return this;
        }

        public ReportParameterBuilder WithDependency(string dependent)
        {
            _dependencies = new[] {dependent};
            return this;
        }

        public ReportParameterBuilder WithValidValues(params ValidValue[] validValues)
        {
            _validValues = validValues;
            return this;
        }

        public ReportParameterBuilder WithValidValues2(params ValidValue[] validValues)
        {
            _validValues = validValues;
            return this;
        }

        public ReportParameterBuilder WithMultiValueFlag(bool flag)
        {
            _multiValueFlag = flag;
            return this;
        }

        public ReportParameterBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public ReportParameterBuilder WithPrompt(string prompt)
        {
            _prompt = prompt;
            return this;
        }

        public ReportParameterBuilder WithDefaultValues(params string[] values)
        {
            _defaultValue = values;
            return this;
        }

        public ReportParameterBuilder WithPromptUser(bool promptUser)
        {
            _promptUser = promptUser;
            return this;
        }

        public ReportParameter Build()
        {
            return new ReportParameter
                       {
                           Name = _name,
                           MultiValue = _multiValueFlag,
                           ValidValues = _validValues,
                           Dependencies = _dependencies,
                           Prompt = _prompt,
                           DefaultValues = _defaultValue,
                           PromptUser = _promptUser
                       };
        }
    }
}
