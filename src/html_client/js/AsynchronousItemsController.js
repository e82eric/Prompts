var AsynchronousItemsController = Class.extend({
	init: function (createViewFunc) {
		this.items = [];
		this.createViewFunc = createViewFunc;
	},

    setItems: function (items) {
        this.items = items;
		if(this.view != undefined) {
        	this.view.renderItems(this.items);
		}
    },

    setView: function (val) {
        this.view = val;
		if(this.items.length != 0) {
			this.view.renderItems(this.items);
		}
		return this.view;
    },

    createView: function () {
    	this.setView(this.createViewFunc(this));
    	return this.view;
    }
});
