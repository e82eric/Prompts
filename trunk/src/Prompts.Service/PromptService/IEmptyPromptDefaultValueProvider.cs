using System.Collections.Generic;
using Prompts.Service.ReportExecution;

namespace Prompts.Service.PromptService
{
    public interface IEmptyPromptDefaultValueProvider
    {
        IEnumerable<DefaultValue> Get(ReportParameter parameter);
    }
}