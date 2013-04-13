using Prompts.Service.ReportExecution;

namespace Prompts.Service.PromptService.Implementation
{
    public class HierarchyPromptService : IHierarchyPromptService
    {
        private readonly IPromptReportParameterService _parameterService;
        private readonly IHierarchyPromptProvider _hierarchyPromptProvider;

        public HierarchyPromptService(
            IPromptReportParameterService parameterService, 
            IHierarchyPromptProvider hierarchyPromptProvider)
        {
            _hierarchyPromptProvider = hierarchyPromptProvider;
            _parameterService = parameterService;
        }

        public IHierarchyPrompt GetHierarchyPrompt(string promptName, ParameterValue[] parameterValues)
        {
            var parameters = _parameterService.GetParametersFor(promptName, parameterValues);
            return _hierarchyPromptProvider.Get(parameters);
        }
    }
}