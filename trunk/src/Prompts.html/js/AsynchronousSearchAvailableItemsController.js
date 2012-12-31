var AsynchronousSearchAvailableItemsController = AsynchronousItemsController.extend({
    init: function(selector) {
        this.selector = selector;
    },

    createView: function () {
        this.view = new RootItemsView(this);
        return this.view;
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