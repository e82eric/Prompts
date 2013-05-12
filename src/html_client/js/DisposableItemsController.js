var DisposableItemsController = AsynchronousItemsController.extend({
	init: function (itemsDisposer) {
		this.itemsDisposer = itemsDisposer;
	},

    setItems: function (val) {
		if(this.items != undefined) {
       		this.itemsDisposer.dispose(this.items); 
		}
        this._super(val);
    },

    createView: function (createFunc) {
        return this.setView(createFunc(this));
    }
});
