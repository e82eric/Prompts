using System.Collections.Generic;
using Prompts.Service.ReportExecution;

namespace Prompts.Service.PromptService
{
    public interface ISelectionParameterValueBuilder
    {
        ParameterValue[] Get(ReportParameter[] baseReportParmaeters, IEnumerable<PromptSelectionInfo> promptSelectionInfos);
    }
}