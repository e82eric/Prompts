var AsynchronousSearchAvailableItemsController = AsynchronousItemsController.extend({
    init: function(selector) {
        this.selector = selector;
        this._super();
        this.selectedItems = [];
    },

    createView: function () {
        return this.setView(new ItemsView(this, "rootItems"));
    },

    select: function (shiftKeyPressed, controlKeyPressed, item) {
        this.selectedItems = this.selector.select(shiftKeyPressed, controlKeyPressed, this.items, item);
    },

    getSelectedItems: function () {
        return this.selectedItems;
    },

    setItems: function (items) {
        _.each(
            this.items,
            function (item) {
                item.deleteItem();
            },
            this
        );

        this._super(items);
    }
});