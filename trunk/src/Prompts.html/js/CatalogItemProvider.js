function CatalogItemProvider(folderCatalogItemProvider, reportCatalog, promptsController) {
    this.reportCatalog = reportCatalog;
    this.folderCatalogItemProvider = folderCatalogItemProvider;
    this.promptsController = promptsController;

	this.GetItem = function (catalogItem) {
        var model;
        if (catalogItem.Type === "Report") {
            model = new ReportCatalogItemController(catalogItem, this.promptsController);
            model.setRepository(this.reportCatalog);
            model.Children = [];
        }
        else if (catalogItem.Type === "Folder") {
            model = this.folderCatalogItemProvider.GetItem(catalogItem);
        }

		return model;
	}
}

