using Prompts.Service.ReportService;

namespace Prompts.Service.ReportCatalogService
{
    public class NativeCatalogItemInfoMapper : ICatalogItemInfoMapper
    {
        public CatalogItemInfo MapFromCatalogItem(CatalogItem catalogItem)
        {
            CatalogItemType type;

            if (catalogItem.Type == ItemTypeEnum.Report)
            {
                type = CatalogItemType.Report;
            }
            else if (catalogItem.Type == ItemTypeEnum.Folder)
            {
                type = CatalogItemType.Folder;
            }
            else
            {
                type = CatalogItemType.Other;
            }

            return new CatalogItemInfo(catalogItem.Name, catalogItem.Path, type);
        }
    }
}