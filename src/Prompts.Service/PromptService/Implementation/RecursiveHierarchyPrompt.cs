using Prompts.Service.ReportExecution;

namespace Prompts.Service.PromptService.Implementation
{
    public class RecursiveHierarchyPrompt : IHierarchyPrompt
    {
        private readonly ReportParameter[] _parameters;

        public RecursiveHierarchyPrompt(ReportParameter[] parameters)
        {
            _parameters = parameters;
        }

        public PromptLevel GetChildOf(string parameterName)
        {
            return new PromptLevel(parameterName, _parameters[1].ValidValues, true);
        }
    }
}