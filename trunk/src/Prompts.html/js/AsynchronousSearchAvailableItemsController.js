var AsynchronousSearchAvailableItemsController = AvailableItemsControllerBase.extend({
    init: function(selector) {
        this.selector = selector;
    },

    createView: function () {
        this.view = new AvailableItemsView(this);
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

        this.items = items;

        this.view.renderItems(this.items);
    }
});