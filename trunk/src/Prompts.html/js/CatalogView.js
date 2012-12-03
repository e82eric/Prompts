var CatalogView = Backbone.View.extend({
    css: "childItems",
    tagName: "ul",

	initialize: function (options) {
        this.css = options.css;
        this.render();
	},

	render: function () {
        this.$el.attr("class", this.css);

		this.collection.each(function (catalogItem) {
			this.renderCatalogItem(catalogItem);
		}, this);
	},

	renderCatalogItem: function (catalogItem) {
		var catalogItemView;

		if (catalogItem instanceof  ReportCatalogItem) {
			catalogItemView = new CatalogItemView({ model: catalogItem});
		} else if (catalogItem instanceof FolderCatalogItem) {
			catalogItemView = new FolderCatalogItemView({ model: catalogItem});
		} else if (catalogItem instanceof EmptyFolderCatalogItem) {
            catalogItemView = new EmptyFolderCatalogItemView({ model: catalogItem});
        }

		this.$el.append(catalogItemView.el);
	}
});