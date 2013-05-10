var DisposableItemsController = AsynchronousItemsController.extend({
    setItems: function (val) {
        _.each(
            this.items,
            function (item) {
                item.deleteItem();
            }
        );

        this._super(val);
    },

    createView: function (createFunc) {
        return this.setView(createFunc(this));
    }
});