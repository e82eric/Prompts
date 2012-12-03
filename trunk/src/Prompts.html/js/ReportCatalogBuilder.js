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