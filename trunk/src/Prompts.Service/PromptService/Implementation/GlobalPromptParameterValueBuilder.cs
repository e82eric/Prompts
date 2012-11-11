using System.Collections.Generic;
using Prompts.Service.ReportExecution;

namespace Prompts.Service.PromptService.Implementation
{
    public class GlobalPromptParameterValueBuilder : IParameterValueBuilder
    {
        private readonly ReportParameter _valueParameter;
        private readonly ReportParameter _labelParameter;

        public GlobalPromptParameterValueBuilder(ReportParameter valueParameter, ReportParameter labelParameter)
        {
            _labelParameter = labelParameter;
            _valueParameter = valueParameter;
        }

        public string PromptName
        {
            get { return _valueParameter.Name; }
        }

        public IEnumerable<ParameterValue> BuildParameterValuesFor(IEnumerable<ValidValue> selections)
        {
            var parameterValues = new List<ParameterValue>();

            foreach (var selection in selections)
            {
                var valueParameterValue = CreateParmaeterValue(_valueParameter.Name, selection.Value);
                var labelParameterValue = CreateParmaeterValue(_labelParameter.Name, selection.Label);
                parameterValues.Add(valueParameterValue);
                parameterValues.Add(labelParameterValue);
            }

            return parameterValues;
        }

        private static ParameterValue CreateParmaeterValue(string name, string value)
        {
            return new ParameterValue { Value = value, Name = name };
        }
    }
}