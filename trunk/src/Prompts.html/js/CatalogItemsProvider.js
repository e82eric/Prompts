function CatalogItemsProvider (catalogItemProvider) {
	this.catalogItemProvider = catalogItemProvider;

	this.GetItems = function(catalogItems) {
		var result = [];
		
		_.each(catalogItems, function (item) {
			var providerItem = this.catalogItemProvider.GetItem(item);
			result.push(providerItem);
		},
		this);

		return new Catalog(result);
	}
}