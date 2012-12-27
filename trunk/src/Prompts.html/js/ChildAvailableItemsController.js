var ChildAvailableItemsController = AvailableItemsControllerBase.extend({
    init: function() {
    },

    createView: function () {
        this.view = new ChildAvailableItemsView(this);
        return this.view;
    }
});