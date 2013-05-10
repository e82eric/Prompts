var AsynchronousSearchAvailableItemsController = DisposableItemsController.extend({
    init: function(selector) {
        this.selector = selector;
        this._super();
        this.selectedItems = [];
    },

    select: function (shiftKeyPressed, controlKeyPressed, item) {
        this.selectedItems = this.selector.select(shiftKeyPressed, controlKeyPressed, this.items, item);
    },

    getSelectedItems: function () {
        return this.selectedItems;
    }
});