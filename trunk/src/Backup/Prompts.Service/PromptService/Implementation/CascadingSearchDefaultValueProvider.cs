using System.Collections.Generic;
using System.Linq;
using Prompts.Service.ReportExecution;

namespace Prompts.Service.PromptService.Implementation
{
    public class CascadingSearchDefaultValueProvider : ICascadingSearchDefaultValueProvider
    {
        private readonly IPromptReportParameterService _promptReportParameterService;
        private readonly IDefaultValueProvider _defaultValueProvider;

        public CascadingSearchDefaultValueProvider(
            IPromptReportParameterService promptReportParameterService, 
            IDefaultValueProvider defaultValueProvider)
        {
            _defaultValueProvider = defaultValueProvider;
            _promptReportParameterService = promptReportParameterService;
        }

        public IEnumerable<DefaultValue> Get(
            string promptReportName, 
            string searchParameterName,
            string[] valueParameterDefaults, 
            string[] labelParameterDefaults)
        {
            if(valueParameterDefaults.Length == 0 || labelParameterDefaults.Length == 0)
            {
                return new List<DefaultValue>();
            }

            var valueParameterDefault = valueParameterDefaults.First();
            var labelParameterDefault = labelParameterDefaults.First();

            var defaultValue = _defaultValueProvider.Get(valueParameterDefault, labelParameterDefault);

            var resultParameterValidValues = _promptReportParameterService.GetParametersFor(
                promptReportName, new ParameterValue {Name = searchParameterName, Value = labelParameterDefault})
                [1].ValidValues;

            var firstMatchingValue = resultParameterValidValues.Where(
                v =>
                v.Value == defaultValue.Value &&
                v.Label == labelParameterDefault)
                .FirstOrDefault();

            if(firstMatchingValue == null)
            {
                return new List<DefaultValue>();
            }

            return new[] {_defaultValueProvider.Get(valueParameterDefault, labelParameterDefault)};
        }
    }
}