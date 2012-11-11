using System.Collections.Generic;
using Prompts.Service.ReportExecution;

namespace Prompts.Service.PromptService
{
    public interface IPromptSelections
    {
        IEnumerable<ParameterValue> CreateParameterValuesFor(IParameterValueBuilder parameterValueBuilder);
    }
}