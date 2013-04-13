using ServiceStack.ServiceInterface;

namespace Prompts.Service.ReportCatalogService
{
    public class ReportsService : RestServiceBase<object>
    {
        private readonly ReportCatalogService _reportCatalogService;

        public ReportsService(ReportCatalogService reportCatalogService)
        {
            _reportCatalogService = reportCatalogService;
        }

        public override object OnGet(object request)
        {
            return _reportCatalogService.GetReportCatalogInfo();
        }
    }
}