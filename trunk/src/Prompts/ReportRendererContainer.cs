using System;
using System.Windows;
using Prompts.PromptServiceProxy;
using Prompts.ReportRendering;
using Prompts.ReportRendering.Implementation;
using Prompts.ReportRendering.ViewModel;

namespace Prompts
{
    public class ReportRendererContainer : IReportRendererContainer
    {
        public virtual IReportRenderer Create()
        {
            return CreateReportsViewModel(CreateReportsViewModelBuilder());
        }

        protected virtual IReportRenderer CreateReportsViewModel(IReportViewModelBuilder reportViewModelBuilder)
        {
            return new PopupReportViewModel(CreateReportsViewModelBuilder());
        }

        private IReportViewModelBuilder CreateReportsViewModelBuilder()
        {
            return CreateReportViewModelBuilder(
                CreateServerName(),
                CreateReportExecutionService());
        }

        protected IReportExecutionService CreateReportExecutionService()
        {
            return new ReportExecutionService(ServiceInjector.Inject<IPromptSelectionServiceClient>());
        }

        protected virtual IReportViewModelBuilder CreateReportViewModelBuilder(
            string serverName, 
            IReportExecutionService reportExecutionService)
        {
            return new ReportViewModelBuilder(serverName, reportExecutionService);
        }

        protected static string CreateServerName()
        {
            if (Application.Current.Host.Source != null)
            {
                return string.Format(
                    "{0}:{1}",
                    Application.Current.Host.Source.DnsSafeHost,
                    Application.Current.Host.Source.Port);
            }
            throw new Exception(
                "An exception was thrown while trying to resole: 'Application.Current.Host.Source'");

        }
    }
}
