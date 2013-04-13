using Prompts.Service.ReportExecution;

namespace Prompts.Service.PromptService
{
    public interface IEmbeddedPromptProvider<out T>
    {
        T Get(ReportParameter parameter);
    }
}