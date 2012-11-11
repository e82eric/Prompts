using System.Collections.Generic;
using System.Linq;
using Prompts.Service.ReportExecution;

namespace Prompts.Service.PromptService.Implementation
{
    public class PromptSelections : IPromptSelections
    {
        private readonly IEnumerable<PromptSelectionInfo> _enumeration;

        public PromptSelections(IEnumerable<PromptSelectionInfo> enumeration)
        {
            _enumeration = enumeration;
        }

        public IEnumerable<ParameterValue> CreateParameterValuesFor(IParameterValueBuilder parameterValueBuilder)
        {
            var selectionInfo = _enumeration.Where(i => i.PromptName == parameterValueBuilder.PromptName).Single();
            return parameterValueBuilder.BuildParameterValuesFor(selectionInfo.Selections);
        }
    }
}