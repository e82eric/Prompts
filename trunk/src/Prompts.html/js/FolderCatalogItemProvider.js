function FolderCatalogItemProvider() {
    this.catalogItemsProvider = undefined;

    this.setCatalogItemsProvider = function (val) {
        this.catalogItemsProvider = val;
    };

    this.GetItem = function (catalogItem) {
        var children;
        var model;

        if (catalogItem.Children.length != 0) {
            model = new FolderCatalogItemController(catalogItem);
            children = this.catalogItemsProvider.GetItems(catalogItem.Children);
            model.Children = children;
        } else {
            model = new EmptyFolderCatalogItemController(catalogItem);
            model.Children = [];
        }

        return model;
    }
}