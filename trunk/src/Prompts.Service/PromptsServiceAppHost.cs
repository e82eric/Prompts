using Funq;
using Prompts.Service.PromptService;
using Prompts.Service.PromptService.Construction;
using Prompts.Service.ReportCatalogService;
using Prompts.Service.ReportCatalogService.Construction;
using ServiceStack.WebHost.Endpoints;

namespace Prompts.Service
{
    public class PromptsServiceAppHost : AppHostBase
    {
        public PromptsServiceAppHost() : base("Prompts Service", typeof(ReportsService).Assembly) { }

        public override void Configure(Container container)
        {
            Container.Register(c => PromptServiceInjector.Inject());
            Container.Register(c => ChildPromptLevelServiceInjector.Inject());
            Container.Register(c => ReportCatalogServiceInjector.Inject());
            Container.Register(c => PromptSelectionServiceInjector.Inject());
            Container.Register(c => ChildPromptLevelServiceInjector.InjectRecursive());

            Routes
                .Add<object>("/reports")
                .Add<PromptsRequest>("/prompts")
                .Add<PromptsRequest>("/prompts/{Path}")
                .Add<ChildPromptItemsRequest>("/prompts/child_items")
                .Add<RecursiveChildPromptItemsRequest>("/prompts/child_items/recursive")
                .Add<SetPromptSelectionsRequest>("/prompts/set_prompt_selections");
        }
    }
}