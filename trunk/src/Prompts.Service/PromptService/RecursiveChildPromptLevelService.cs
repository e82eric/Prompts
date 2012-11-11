using ServiceStack.ServiceInterface;

namespace Prompts.Service.PromptService
{
    public class RecursiveChildPromptLevelService : RestServiceBase<RecursiveChildPromptItemsRequest>
    {
        private readonly IHierarchyPromptService _recursiveHierarchyPromptService;

        public RecursiveChildPromptLevelService(IHierarchyPromptService recursiveHierarchyPromptService)
        {
            _recursiveHierarchyPromptService = recursiveHierarchyPromptService;
        }

        public override object OnPost(RecursiveChildPromptItemsRequest request)
        {
            var hierarhcy = _recursiveHierarchyPromptService.GetHierarchyPrompt(
                request.PromptName, 
                new[] { request.ParameterValue });
            var promptLevel = hierarhcy.GetChildOf(request.ParameterName);
            return new GetChildrenResponse
                {
                    ErrorMessage = string.Empty, 
                    ErrorOccured = false, PromptLevel = 
                    promptLevel
                };
        }
    }
}