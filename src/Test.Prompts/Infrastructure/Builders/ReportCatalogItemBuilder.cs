using Moq;
using Prompts.ReportCatalog.ViewModels;

namespace Test.Prompts.Infrastructure.Builders
{
    public class ReportCatalogItemBuilder
    {
        private string _reportName = "Report Name";

        public ReportCatalogItemBuilder WithReportName(string name)
        {
            _reportName = name;
            return this;
        }

        public IReportCatalogItemViewModel Build()
        {
            var mock = new Mock<IReportCatalogItemViewModel>();
            mock.SetupGet(m => m.ReportName).Returns(_reportName);
            return mock.Object;
        }
    }
}
