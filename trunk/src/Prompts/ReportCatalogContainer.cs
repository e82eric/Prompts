using System;
using System.Windows;
using System.Windows.Media;
using Prompts.Prompting.ViewModels;
using Prompts.ReportCatalog.Model;
using Prompts.ReportCatalog.ViewModels;
using Prompts.ReportCatalog.ViewModels.Implementation;
using Prompts.ReportCatalog.Views;

namespace Prompts
{
    public class ReportCatalogContainer
    {
        public IPromptsViewModel PromptsViewModel { get; set; }

        public Color TextColor { get; set; }

        public ReportCatalogContainer()
        {
            TextColor = Colors.Black;
        }

        public ReportCatalogControl Create()
        {
            return CreateReportCatalogControl(InjectReportCatalogViewModel());
        }

        protected virtual ReportCatalogControl CreateReportCatalogControl(
            IReportCatalogViewModel reportCatalogItemViewModel)
        {
            return new ReportCatalogControl(reportCatalogItemViewModel, new SolidColorBrush(TextColor));
        }

        public IReportCatalogViewModel InjectReportCatalogViewModel()
        {
            return CreateReportCatalogViewModel(CreateReportCatalogService());
        }

        protected virtual IReportCatalogViewModel CreateReportCatalogViewModel(
            IReportCatalogViewModelService reportCatalogViewModelService)
        {
            return new ReportCatalogViewModel(reportCatalogViewModelService);
        }

        private IReportCatalogViewModelService CreateReportCatalogService()
        {
            return new ReportCatalogViewModelService(
                CreateReportCatalogServiceClient()
                , CreateReportCatalogBuilder());
        }

        private ICatalogItemViewModelBuilder CreateReportCatalogBuilder()
        {
            return new CatalogItemViewModelBuilder(PromptsViewModel);
        }

        private static IReportCatalogServiceClient CreateReportCatalogServiceClient()
        {
            const string absoluteServiceUri = "/Prompts.Service/api/Reports";
            string uri;
            if (Application.Current.Host.Source != null)
            {
                uri = new Uri(Application.Current.Host.Source, absoluteServiceUri).AbsoluteUri;
            }
            else
            {
                throw new Exception(
                    "An excpetion occured while trying to resolve 'Application.Current.Host.Source'");
            }


            return ServiceInjector.Inject<IReportCatalogServiceClient>();
        }
    }
}
