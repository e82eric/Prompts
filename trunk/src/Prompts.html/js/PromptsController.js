var PromptsController = AsynchronousItemsController.extend({
    setItems: function (val) {
        _.each(
            this.items,
            function (item) {
                item.delete();
            }
        );

        this._super(val);
    },

    createView: function () {
        this.view = new PromptsView(this);
        return this.view;
    }
});