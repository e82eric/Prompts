var ReportCatalogItemBuilder = Class.extend({
    init: function (folderCatalogItemBuilder, promptsController, rootItemsController) {
        this.folderCatalogItemBuilder = folderCatalogItemBuilder;
        this.promptsController = promptsController;
        this.rootItemsController = rootItemsController;
    },

    build: function (buildParams) {
        var model = buildParams.model;

        var controller = undefined;
        if (model.Type === "Report") {
            controller = new ReportCatalogItemController(model, this.promptsController, this.rootItemsController);
        }
        else if (model.Type === "Folder") {
            controller = this.folderCatalogItemBuilder.build(model);
        }

		return controller;
	}
});

