using Prompts.Service.ReportService;

namespace Prompts.Service.ReportCatalogService
{
    public interface ICatalogItemInfoMapper
    {
        CatalogItemInfo MapFromCatalogItem(CatalogItem catalogItem);
    }
}