using Prompts.Service.ReportExecution;

namespace Prompts.Service.PromptService
{
    public interface IHierarchyPromptInfoProvider
    {
        PromptInfo GetPromptInfo(GlobalPromptBaseReportInfo baseReportInfo, ReportParameter[] promptReportParameters);
    }
}