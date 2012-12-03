function CatalogItemProvider(folderCatalogItemProvider, reportCatalog) {
    this.reportCatalog = reportCatalog;
    this.folderCatalogItemProvider = folderCatalogItemProvider;

	this.GetItem = function (catalogItem) {
        var model;
        if (catalogItem.Type === "Report") {
            model = new ReportCatalogItem(catalogItem);
            model.setReportCatalog(this.reportCatalog);
            model.set("Children", new Backbone.Collection());
        }
        else if (catalogItem.Type === "Folder") {
            model = this.folderCatalogItemProvider.GetItem(catalogItem);
        }

		return model;
	}
}

