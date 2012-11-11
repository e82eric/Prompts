using Prompts.Service.ReportExecution;

namespace Prompts.Prompting.Construction
{
    public interface IPromptItemProvider<out T>
    {
        T Get(string promptName, string parameterName, ValidValue validValue);
    }
}