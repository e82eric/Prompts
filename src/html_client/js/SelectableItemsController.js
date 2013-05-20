var SelectableItemsController = DisposableItemsController.extend({
    init: function(selector, itemsDisposer) {
        this.selector = selector;
        this._super(itemsDisposer);
        this.selectedItems = [];
    },

    select: function (shiftKeyPressed, controlKeyPressed, item) {
		var itemsToSelectWith = undefined;

		if(this.displayItems == undefined) {
			itemsToSelectWith = this.items;
		} else {
			itemsToSelectWith = this.displayItems;
		}

        this.selectedItems = this.selector.select(shiftKeyPressed, controlKeyPressed, itemsToSelectWith, item);
    },

    getSelectedItems: function () {
        return this.selectedItems;
    }
});