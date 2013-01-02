using System.Collections.Generic;
using Prompts.Service.ReportExecution;

namespace Prompts.Service.PromptService
{
    public interface IBaseReportInterpreter<out T>
    {
        IEnumerable<T> Get(ReportParameter[] baseReportParameters);
    }
}