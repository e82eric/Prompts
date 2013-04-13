using Prompts.Service.ReportExecution;

namespace Prompts.Service.PromptService
{
    public interface ISingleLevelPromptInfoProvider
    {
        PromptInfo GetPromptInfo(GlobalPromptBaseReportInfo baseReportInfo, ReportParameter promptReportParameter);
    }
}