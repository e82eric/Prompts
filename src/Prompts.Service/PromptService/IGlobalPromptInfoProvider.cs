using Prompts.Service.ReportExecution;

namespace Prompts.Service.PromptService
{
    public interface IGlobalPromptInfoProvider
    {
        PromptInfo GetPromptInfo(GlobalPromptBaseReportInfo baseReportInfo, ReportParameter[] promptReportParameters);
    }
}