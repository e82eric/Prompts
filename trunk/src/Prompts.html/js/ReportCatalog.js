$(function () {
    var reportCatalogBuilder = new ReportCatalogBuilder();
	var catalogRepository = new CatalogRepository(reportCatalogBuilder);
	var loadingPanel = new ReportCatalogPanel({reportCatalogRepository: catalogRepository});

    new ReportCatalogPanelView({model:loadingPanel});
});

function ReportCatalog (hierarchyFlattener, singleSelector) {
    this.hierarchyFlattener = hierarchyFlattener;
    this.singleSelector = singleSelector;

    this.setItems = function (val) {
        this.items = val;
    }

    this.Select = function (item) {
        var flattenedItems = this.hierarchyFlattener.Flatten(this.items);
        this.singleSelector.Select(flattenedItems, item);
    }
}

function ReportCatalogBuilder () {
    this.Build = function(jsonItems) {
        var hierarchyFlattener = new HierarchyFlattener();

        var folderCatalogItemProvider = new FolderCatalogItemProvider();

        var reportCatalog = new ReportCatalog(
            new HierarchyFlattener(),
            new SingleSelector());

        var catalogItemProvider = new CatalogItemProvider(folderCatalogItemProvider, reportCatalog);
        var rootCatalogItemsProvider = new CatalogItemsProvider(catalogItemProvider);
        var catalogItemsProvider = new CatalogItemsProvider(catalogItemProvider);
        folderCatalogItemProvider.setCatalogItemsProvider(catalogItemsProvider);

        var catalogItems = catalogItemsProvider.GetItems(jsonItems);

        reportCatalog.setItems(catalogItems);

        return reportCatalog;
    }

}
