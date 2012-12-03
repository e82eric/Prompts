function FolderCatalogItemProvider() {
    this.catalogItemsProvider = undefined;

    this.setCatalogItemsProvider = function (val) {
        this.catalogItemsProvider = val;
    };

    this.GetItem = function (catalogItem) {
        var children;
        var model;

        if (catalogItem.Children.length != 0) {
            model = new FolderCatalogItem(catalogItem);
            children = this.catalogItemsProvider.GetItems(catalogItem.Children);
            model.set("Children", children);
        } else {
            model = new EmptyFolderCatalogItem(catalogItem);
        }

        return model;
    }
}