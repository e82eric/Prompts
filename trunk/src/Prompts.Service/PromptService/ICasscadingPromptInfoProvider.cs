using Prompts.Service.ReportExecution;

namespace Prompts.Service.PromptService
{
    public interface ICasscadingPromptInfoProvider
    {
        PromptInfo GetPromptInfo(
            GlobalPromptBaseReportInfo baseReportInfo, 
            ReportParameter searchParameter, 
            ReportParameter resultParameter);
    }
}