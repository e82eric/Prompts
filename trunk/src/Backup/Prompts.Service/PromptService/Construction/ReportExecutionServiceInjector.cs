using System.Net;
using Prompts.Service.Properties;
using Prompts.Service.ReportExecution;

namespace Prompts.Service.PromptService.Construction
{
    public class ReportExecutionServiceInjector
    {
        private static readonly IReportExecutionService ReportExecutionService = CreateReportExecutionService();

        public static IReportExecutionService Inject()
        {
            return ReportExecutionService;
        }

        private static IReportExecutionService CreateReportExecutionService()
        {
            var executionService = new ReportExecutionService(
                InjectServiceUrl(), 
                CredentialCache.DefaultCredentials);
            return executionService;
        }

        private static string InjectServiceUrl()
        {
            return string.Format(@"{0}\reportexecution2005.asmx", Settings.Default.ReportServerUrl);
        }
    }
}