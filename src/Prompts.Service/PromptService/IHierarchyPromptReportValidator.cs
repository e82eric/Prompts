using Prompts.Service.ReportExecution;

namespace Prompts.Service.PromptService
{
    public interface IHierarchyPromptReportValidator
    {
        void Validate(string promptName, ReportParameter[] promptReportParameters);
    }
}