using System.Linq;
using Prompts.Service.ReportExecution;

namespace Prompts.Service.PromptService.Implementation
{
    public class HierarchyPrompt : IHierarchyPrompt
    {
        private readonly ReportParameter[] _parameters;

        public HierarchyPrompt(ReportParameter[] parameters)
        {
            _parameters = parameters;
        }

        public PromptLevel GetChildOf(string parameterName)
        {
            var childParameter = GetChildParameterOrDefault(parameterName);

            var childOfChild = GetChildParameterOrDefault(childParameter.Name);

            var hasChild = childOfChild != null ? true : false;

            return new PromptLevel(childParameter.Name, childParameter.ValidValues, hasChild);
        }

        private ReportParameter GetChildParameterOrDefault(string parameterName)
        {
            var parametersWithDepencies = _parameters.Where(p => p.Dependencies != null);

            return parametersWithDepencies.Where(p => p.Dependencies.Single().Equals(parameterName)).SingleOrDefault();
        }
    }
}