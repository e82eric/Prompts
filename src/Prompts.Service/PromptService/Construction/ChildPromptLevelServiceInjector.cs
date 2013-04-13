using Prompts.Service.PromptService.Implementation;

namespace Prompts.Service.PromptService.Construction
{
    public class ChildPromptLevelServiceInjector
    {
        public static ChildPromptLevelService Inject()
        {
            return new ChildPromptLevelService(
                InjectHierarchyPromptService());
        }

        public static RecursiveChildPromptLevelService InjectRecursive()
        {
            return new RecursiveChildPromptLevelService(InjectRecursiveHierarchyPromptService());
        }

        private static IHierarchyPromptService InjectHierarchyPromptService()
        {
            return new HierarchyPromptService(
                PromptReportParameterServiceInjector.Inject(),
                new HierarchyPromptProvider());
        }

        private static IHierarchyPromptService InjectRecursiveHierarchyPromptService()
        {
            return new HierarchyPromptService(
                PromptReportParameterServiceInjector.Inject(),
                new RecursiveHierarchyPromptProvider());
        }
    }
}