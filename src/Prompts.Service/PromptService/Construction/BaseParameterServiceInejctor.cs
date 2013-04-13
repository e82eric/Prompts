using Prompts.Service.PromptService.Implementation;

namespace Prompts.Service.PromptService.Construction
{
    public static class BaseParameterServiceInejctor
    {
        private static readonly IBaseReportParameterService BaseReportParameterService = CreateBaseReportParameterService();

        public static IBaseReportParameterService Inject()
        {
            return BaseReportParameterService;
        }

        public static IBaseReportParameterService CreateBaseReportParameterService()
        {
            return new BaseReportParameterService(ReportExecutionServiceInjector.Inject());
        }
    }
}