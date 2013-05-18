var FolderCatalogItemBuilder = Class.extend({
    init: function () {
    },

    build: function (model) {
        var controller = undefined;

        if (model.Children.length != 0) {
            var childItems = this.childItemsBuilder.build(model.Children);
            var childItemsController = new AsynchronousItemsController(function (controller) { return new ItemsView(this, "childItems"); });
            childItemsController.setItems(childItems);
            controller = new FolderCatalogItemController(model, childItemsController);
            controller.setChildren(childItems);
        } else {
            controller = new EmptyFolderCatalogItemController(model);
        }

        return controller;
    },

    setChildItemsBuilder: function (val) {
        this.childItemsBuilder = val;
    }
});