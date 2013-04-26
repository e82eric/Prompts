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

    evaluateReadyForExecution: function () {
        var flag = _.where(this.items, {readyForExecution: false}).length == 0;

        if(flag) {
            this.view.setExecutionIndicatorReady();
        } else {
            this.view.setExecutionIndicatorNotReady();
        }
    },

    onExecuteClick: function () {
        var selections = _.map(this.items, function (item) { return item.selectionInfo(); });
        alert(selections);
    },

    createView: function () {
        this.view = new PromptsView(this);
        return this.view;
    }
});