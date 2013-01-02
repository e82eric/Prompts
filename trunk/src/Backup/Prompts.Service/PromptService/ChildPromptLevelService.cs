using ServiceStack.ServiceInterface;

namespace Prompts.Service.PromptService
{
    public class ChildPromptLevelService : RestServiceBase<ChildPromptItemsRequest>
    {
        private readonly IHierarchyPromptService _hierarchyPromptService;

        public ChildPromptLevelService(IHierarchyPromptService hierarchyPromptService)
        {
            _hierarchyPromptService = hierarchyPromptService;
        }

        public override object OnPost(ChildPromptItemsRequest request)
        {
            var hierarhcy = _hierarchyPromptService.GetHierarchyPrompt(request.PromptName, request.ParameterValues);
            return hierarhcy.GetChildOf(request.ParameterName);
        }
    }
}
