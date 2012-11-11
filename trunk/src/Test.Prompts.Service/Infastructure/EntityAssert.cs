using System;
using NUnit.Framework;
using Prompts.Service.ReportCatalogService;
using Prompts.Service.ReportService;

namespace Test.Prompts.Service.Infastructure
{
    static class EntityAssert
    {
        public static void AssertMappedCorrectlyFrom(this CatalogItemInfo source, CatalogItem catalogItem)
        {
            switch (catalogItem.Type)
            {
                case ItemTypeEnum.Report:
                    Assert.AreEqual(CatalogItemType.Report, source.Type);
                    break;
                case ItemTypeEnum.Folder:
                    Assert.AreEqual(CatalogItemType.Folder, source.Type);
                    break;
                default:
                    throw new Exception();
            }

            Assert.AreEqual(catalogItem.Name, source.Name);
            Assert.AreEqual(catalogItem.Path, source.Path);
        }

        public static CatalogItemInfo ExpectedCatalogInfo(this CatalogItem source)
        {
           return new CatalogItemInfo(source.Name, source.Path, DetermineCatalogItemType(source.Type));
        }

        private static CatalogItemType DetermineCatalogItemType(ItemTypeEnum itemTypeEnum)
        {
            switch (itemTypeEnum)
            {
                case ItemTypeEnum.Report:
                    return CatalogItemType.Report;
                case ItemTypeEnum.Folder:
                    return CatalogItemType.Folder;
                default:
                    throw new Exception();
            }
        }
    }
}
