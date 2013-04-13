using System;
using Prompts.Service.PromptService.Implementation;
using Prompts.Service.Properties;

namespace Prompts.Service.PromptService.Construction
{
    public class PromptReportParameterServiceInjector
    {
        public static IPromptReportParameterService Inject()
        {
            if (Settings.Default.ReportingServicesMode == "Integrated")
            {
                return new PromptReportParameterService(
                    InjectPromptReportServceFolder(),
                    ReportExecutionServiceInjector.Inject(),
                    new IntegratedPromptReportNameParser());
            }
            if (Settings.Default.ReportingServicesMode == "Native")
            {
                return new PromptReportParameterService(
                    InjectPromptReportServceFolder(),
                    ReportExecutionServiceInjector.Inject(),
                    new NativePromptReportNameParser());
            }

            throw new Exception();
        }

        private static IReportServerFolder InjectPromptReportServceFolder()
        {
            var folderPath = Settings.Default.Prompt_Report_Folder;
            return new ReportServerFolder(folderPath);
        }
    }
}