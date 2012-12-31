var ChildAvailableItemsController = AsynchronousItemsController.extend({
    createView: function () {
        this.view = new ChildAvailableItemsView(this);
        return this.view;
    }
});