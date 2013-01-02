using Prompts.Service.ReportService;

namespace Prompts.Service.ReportCatalogService
{
    public class IntegratedCatalogItemInfoMapper : ICatalogItemInfoMapper
    {
        public CatalogItemInfo MapFromCatalogItem(CatalogItem catalogItem)
        {
            var nameWithoutRDL = catalogItem.Name.Replace(".rdl", "");

            CatalogItemType type;

            if(catalogItem.Type == ItemTypeEnum.Report)
            {
                type = CatalogItemType.Report;
            }
            else if(catalogItem.Type == ItemTypeEnum.Folder)
            {
                type = CatalogItemType.Folder;
            }
            else
            {
                type = CatalogItemType.Other;
            }

            return new CatalogItemInfo(nameWithoutRDL, catalogItem.Path, type);
        }
    }
}