using System.Collections.Generic;
using Prompts.Service.ReportExecution;

namespace Prompts.Service.PromptService
{
    public interface IParameterValueBuilder
    {
        IEnumerable<ParameterValue> BuildParameterValuesFor(IEnumerable<ValidValue> selections);
        string PromptName { get; }
    }
}