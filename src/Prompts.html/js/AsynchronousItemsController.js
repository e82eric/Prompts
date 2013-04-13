var AsynchronousItemsController = Class.extend({
	init: function () {
		this.items = [];
	},

    setItems: function (items) {
        this.items = items;
        this.view.renderItems(this.items);
    }
});