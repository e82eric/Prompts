var RootReportCatalogItemsController = AsynchronousItemsController.extend({
    init: function (selector) {
        this.selector = selector;
    },

    select: function (item) {
        this.selector.select(this.items, item);
    },

    setView: function(val) {
        this.view = val;
    },

    createView: function () {
        this.setView(new ItemsView(this, "rootItems"));
        return this.view;
    }
});