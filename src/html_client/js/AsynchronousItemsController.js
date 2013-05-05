var AsynchronousItemsController = Class.extend({
	init: function (createViewFunc) {
		this.items = [];
		this.createViewFunc = createViewFunc;
	},

    setItems: function (items) {
        this.items = items;
        this.view.renderItems(this.items);
    },

    setView: function (val) {
        this.view = val;
        return this.view;
    },

    createView: function () {
    	this.setView(this.createViewFunc(this));
    	return this.view;
    }
});