var AsynchronousItemsController = Class.extend({
    setItems: function (items) {
        this.items = items;
        this.view.renderItems(this.items);
    }
});