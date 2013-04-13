using Prompts.Service.ReportExecution;

namespace Prompts.Service.PromptService
{
    public interface IGlobalPromptBaseReportInfoMapper
    {
        GlobalPromptBaseReportInfo Map(ReportParameter valueParameter, ReportParameter labelParameter);
    }
}