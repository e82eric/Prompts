var ReportCatalogItemsController = Class.extend({
    init: function (items) {
        this.items = items;
    },

    setView: function(val) {
        this.view = val;
    },

    createView: function () {
        this.setView(new ItemsView(this, "childItems"));
        return this.view;
    }
});