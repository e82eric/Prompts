var ReportCatalogPanel = Backbone.Model.extend({
	reportCatalogRepository: undefined,

	initialize: function () {
		this.reportCatalogRepository = this.get('reportCatalogRepository');
	},

	run: function (onSuccess) {
        this.reportCatalogRepository.GetCatalog(function (catalog) {
			var catalogView = new CatalogItemsView({ collection: catalog.items, css: "rootItems" });
			onSuccess(catalogView);
		});
	}
});