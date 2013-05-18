var DisposableItemsController = AsynchronousItemsController.extend({
	init: function (itemsDisposer) {
		this.itemsDisposer = itemsDisposer;
		this._super();
	},

    setItems: function (val) {
		if(this.items != undefined && this.view != undefined) {
       		this.itemsDisposer.dispose(this.items); 
		}
        this._super(val);
    },

    createView: function (createFunc) {
        return this.setView(createFunc(this));
    }
});
