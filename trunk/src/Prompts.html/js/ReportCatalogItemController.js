var ReportCatalogItemController = SelectableItemController.extend({
    init: function(model, promptsController, rootItemsController) {
        this.model = model;
        this.promptsController = promptsController;
        this.rootItemsController = rootItemsController;
        this.Children = [];
    },

    select: function () {
        this._super();
        this.promptsController.load({Path: this.model.Path});
    },

    changeSelect: function() {
        this.rootItemsController.select(this);
    },

    createView: function() {
        this.setView(new ReportCatalogItemView(this));
        return this.view;
    },

    setView: function (val) {
        this.view = val;
    }
});