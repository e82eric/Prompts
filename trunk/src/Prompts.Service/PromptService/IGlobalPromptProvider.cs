using Prompts.Service.ReportExecution;

namespace Prompts.Service.PromptService
{
    public interface IGlobalPromptProvider<out T>
    {
        T Get(ReportParameter valueParmaeter, ReportParameter labelParameter);
    }
}