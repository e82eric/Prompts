using Prompts.Service.PromptService.Implementation;

namespace Prompts.Service.PromptService.Construction
{
    public static class PromptSelectionServiceInjector
    {
        public static PromptSelectionService Inject()
        {
            return new PromptSelectionService(
                BaseParameterServiceInejctor.Inject()
                , InjectSelectionParameterValueBuilder());
        }

        private static IPromptSelectionsProvider InjectPromptSelectionProvider()
        {
            return new PromptSelectionsProvider();
        }

        private static ISelectionParameterValueBuilder InjectSelectionParameterValueBuilder()
        {
            return new SelectionParameterValueBuilder(
                InjectBaseReportInterpreter(),
                InjectPromptSelectionProvider());
        }

        private static IBaseReportInterpreter<IParameterValueBuilder> InjectBaseReportInterpreter()
        {
            return new BaseReportInterpreter<IParameterValueBuilder>(
                InjectGlobalPromptParameterValueBuilder()
                , InjectEmbeddedPromptParameterValueProvider());
        }

        private static IEmbeddedPromptProvider<IParameterValueBuilder> InjectEmbeddedPromptParameterValueProvider()
        {
            return new EmbeddedPromptParameterValueProvider();
        }

        private static IGlobalPromptProvider<IParameterValueBuilder> InjectGlobalPromptParameterValueBuilder()
        {
            return new GlobalPromptParameterValueProvider();
        }
    }
}