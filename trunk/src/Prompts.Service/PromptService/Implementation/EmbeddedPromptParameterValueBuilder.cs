using System.Collections.Generic;
using Prompts.Service.ReportExecution;

namespace Prompts.Service.PromptService.Implementation
{
    public class EmbeddedPromptParameterValueBuilder : IParameterValueBuilder
    {
        private readonly ReportParameter _parameter;

        public EmbeddedPromptParameterValueBuilder(ReportParameter parameter)
        {
            _parameter = parameter;
        }

        public string PromptName
        {
            get { return _parameter.Name; }
        }

        public IEnumerable<ParameterValue> BuildParameterValuesFor(IEnumerable<ValidValue> selections)
        {
            var parameterValues = new List<ParameterValue>();

            foreach (var selection in selections)
            {
                var parameterValue
                    = new ParameterValue { Label = selection.Label, Value = selection.Value, Name = _parameter.Name };
                parameterValues.Add(parameterValue);
            }
            return parameterValues;
        }

    }
}