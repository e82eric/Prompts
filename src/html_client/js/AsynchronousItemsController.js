var AsynchronousItemsController = Class.extend({
	init: function (createViewFunc) {
		this.items = [];
		this.createViewFunc = createViewFunc;
	},

    setItems: function (items) {
        this.items = items;
        this.view.renderItems(this.items);
    },

    createView: function () {
    	this.view = this.createViewFunc(this);
    	return this.view;
    }
});