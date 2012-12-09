function CatalogItemProvider(folderCatalogItemProvider, reportCatalog) {
    this.reportCatalog = reportCatalog;
    this.folderCatalogItemProvider = folderCatalogItemProvider;

	this.GetItem = function (catalogItem) {
        var model;
        if (catalogItem.Type === "Report") {
            model = new ReportCatalogItemController(catalogItem);
            model.setReportCatalog(this.reportCatalog);
            model.Children = new Array();
        }
        else if (catalogItem.Type === "Folder") {
            model = this.folderCatalogItemProvider.GetItem(catalogItem);
        }

		return model;
	}
}

