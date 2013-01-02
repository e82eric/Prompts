using System.Collections.Generic;

namespace Prompts.Service.PromptService
{
    public interface ICascadingSearchDefaultValueProvider
    {
        IEnumerable<DefaultValue> Get(
            string promptReportName, 
            string searchParameterName,
            string[] valueParameterDefaults, 
            string[] labelParameterDefaults);
    }
}