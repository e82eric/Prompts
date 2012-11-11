using Moq;
using NUnit.Framework;
using Prompts.Service.ReportCatalogService;
using Prompts.Service.ReportService;
using Test.Prompts.Service.Infastructure;

namespace Test.Prompts.Service
{
    [TestFixture]
    public class ReportCatalogServiceTest
    {
        private Mock<IReportingService2005> _reportingService2005;
        private ReportCatalogService _service;
        private Mock<ICatalogItemInfoMapper> _catalogItemBuilder;
        private const string RootReportFolder = "/Reports";

        [SetUp]
        public void Setup()
        {
            _reportingService2005 = new Mock<IReportingService2005>();
            _catalogItemBuilder = new Mock<ICatalogItemInfoMapper>();
            _service = new ReportCatalogService(_reportingService2005.Object, RootReportFolder, _catalogItemBuilder.Object);
        }

        [Test]
        public void ReturnsOnlyReportsAndFolders()
        {
            var folderItem = A.CatalogItem().Build();
            var otherItem = A.CatalogItem().Build();
            var reportItem = A.CatalogItem().Build();

            var itemsForServiceToReturn = new[] {folderItem, otherItem, reportItem};

            var folderCatalogItemInfo = A.CatalogItemInfo().WithType(CatalogItemType.Folder).Build();
            var otherItemInfo = A.CatalogItemInfo().WithType(CatalogItemType.Other).Build();
            var reportItemInfo = A.CatalogItemInfo().WithType(CatalogItemType.Report).Build();

            _catalogItemBuilder.Setup(b => b.MapFromCatalogItem(folderItem)).Returns(folderCatalogItemInfo);
            _catalogItemBuilder.Setup(b => b.MapFromCatalogItem(otherItem)).Returns(otherItemInfo);
            _catalogItemBuilder.Setup(b => b.MapFromCatalogItem(reportItem)).Returns(reportItemInfo);

            _reportingService2005.Setup(s => s.ListChildren(RootReportFolder, false)).Returns(itemsForServiceToReturn);

            var reportCatalog = _service.GetReportCatalogInfo();

            reportCatalog.AssetItemsAndLength(folderCatalogItemInfo, reportItemInfo);
        }

        [Test]
        public void RecursivelySetsChildren()
        {
            var rootFolder = A.CatalogItem().WithHiddenFlag(false).Build();
            var subFolder = A.CatalogItem().WithHiddenFlag(false).Build();
            var report = A.CatalogItem().WithHiddenFlag(false).Build();

            var rootItemInfo = A.CatalogItemInfo().WithType(CatalogItemType.Folder).Build();
            var subFolderItemInfo = A.CatalogItemInfo().WithType(CatalogItemType.Folder).Build();
            var reportItemInfo = A.CatalogItemInfo().WithType(CatalogItemType.Report).Build();

            _catalogItemBuilder.Setup(b => b.MapFromCatalogItem(rootFolder)).Returns(rootItemInfo);
            _catalogItemBuilder.Setup(b => b.MapFromCatalogItem(subFolder)).Returns(subFolderItemInfo);
            _catalogItemBuilder.Setup(b => b.MapFromCatalogItem(report)).Returns(reportItemInfo);

            _reportingService2005.Setup(s => s.ListChildren(RootReportFolder, false)).Returns(subFolder.ToArray());
            _reportingService2005.Setup(s => s.ListChildren(subFolder.Path, false)).Returns(report.ToArray());

            var reportCatalog = _service.GetReportCatalogInfo();

            reportCatalog.AssetItemsAndLength(subFolderItemInfo);
            reportCatalog.AssertSingle().Children.AssetItemsAndLength(reportItemInfo);
        }

        [Test]
        public void ItDoesNotIncludeHiddenItems()
        {
            var folder = A.CatalogItem()
                .WithPath(RootReportFolder)
                .WithType(ItemTypeEnum.Folder)
                .WithHiddenFlag(false)
                .Build();

            var subFolder = A.CatalogItem()
                .WithPath(RootReportFolder)
                .WithType(ItemTypeEnum.Folder)
                .WithHiddenFlag(true)
                .Build();

            var folderItemInfo = A.CatalogItemInfo().WithType(CatalogItemType.Folder).Build();
            var subFolderItemInfo = A.CatalogItemInfo().WithType(CatalogItemType.Folder).Build();

            _catalogItemBuilder.Setup(b => b.MapFromCatalogItem(folder)).Returns(folderItemInfo);
            _catalogItemBuilder.Setup(b => b.MapFromCatalogItem(subFolder)).Returns(subFolderItemInfo);

            _reportingService2005
                .Setup(s => s.ListChildren(folder.Path, false))
                .Returns(subFolder.ToArray());

            var reportCatalog = _service.GetReportCatalogInfo();

            reportCatalog.AssertLength(0);
        }

        [Test]
        public void ItDoesNotIncludeHiddenChildItems()
        {
            const string childFolderPath = "Child Folder Path";
            const string subFolderPath = "Sub Folder Path";

            var folder = A.CatalogItem()
                .WithPath(RootReportFolder)
                .WithType(ItemTypeEnum.Folder)
                .WithHiddenFlag(false)
                .Build();

            var subFolder = A.CatalogItem()
                .WithPath(subFolderPath)
                .WithType(ItemTypeEnum.Folder)
                .WithHiddenFlag(false)
                .Build();

            var childFolder = A.CatalogItem()
                .WithPath(childFolderPath)
                .WithType(ItemTypeEnum.Folder)
                .WithHiddenFlag(true)
                .Build();

            var folderItemInfo = A.CatalogItemInfo().WithName("Folder Item Info").WithType(CatalogItemType.Folder).Build();
            var subFolderItemInfo = A.CatalogItemInfo().WithName("Sub Folder Item Info").WithType(CatalogItemType.Folder).Build();
            var childFolderItemInfo = A.CatalogItemInfo().WithName("Child Folder Item Info").WithType(CatalogItemType.Folder).Build();

            _catalogItemBuilder.Setup(b => b.MapFromCatalogItem(folder)).Returns(folderItemInfo);
            _catalogItemBuilder.Setup(b => b.MapFromCatalogItem(subFolder)).Returns(subFolderItemInfo);
            _catalogItemBuilder.Setup(b => b.MapFromCatalogItem(childFolder)).Returns(childFolderItemInfo);

            _reportingService2005
                .Setup(s => s.ListChildren(folder.Path, false))
                .Returns(subFolder.ToArray());

            _reportingService2005
                .Setup(s => s.ListChildren(subFolder.Path, false))
                .Returns(childFolder.ToArray());

            var reportCatalog = _service.GetReportCatalogInfo();

            reportCatalog.AssetItemsAndLength(subFolderItemInfo);
            reportCatalog.AssertSingle().Children.AssertLength(0);
        }

        [Test]
        public void ItDoesNotIncludeTheChildrenOfHiddenItems()
        {
            const string childFolderPath = "Child Folder Path";
            const string subFolderPath = "Sub Folder Path";

            var folder = A.CatalogItem()
                .WithPath(RootReportFolder)
                .WithType(ItemTypeEnum.Folder)
                .WithHiddenFlag(false)
                .Build();

            var subFolder = A.CatalogItem()
                .WithPath(subFolderPath)
                .WithType(ItemTypeEnum.Folder)
                .WithHiddenFlag(true)
                .Build();

            var childFolder = A.CatalogItem()
                .WithPath(childFolderPath)
                .WithType(ItemTypeEnum.Folder)
                .WithHiddenFlag(true)
                .Build();

            var folderItemInfo = A.CatalogItemInfo().WithType(CatalogItemType.Folder).Build();
            var subFolderItemInfo = A.CatalogItemInfo().WithType(CatalogItemType.Folder).Build();
            var childFolderItemInfo = A.CatalogItemInfo().WithType(CatalogItemType.Folder).Build();

            _catalogItemBuilder.Setup(b => b.MapFromCatalogItem(folder)).Returns(folderItemInfo);
            _catalogItemBuilder.Setup(b => b.MapFromCatalogItem(subFolder)).Returns(subFolderItemInfo);
            _catalogItemBuilder.Setup(b => b.MapFromCatalogItem(childFolder)).Returns(childFolderItemInfo);

            _reportingService2005
                .Setup(s => s.ListChildren(folder.Path, false))
                .Returns(subFolder.ToArray());

            _reportingService2005
                .Setup(s => s.ListChildren(subFolder.Path, false))
                .Returns(childFolder.ToArray());

            var reportCatalog = _service.GetReportCatalogInfo();

            reportCatalog.AssertLength(0);
        }
    }
}