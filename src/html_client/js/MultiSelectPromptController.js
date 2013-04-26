var MultiSelectPromptController = PromptController.extend({
    init: function (model, availableItemsController, selectedItemsController, createViewFunc) {
        this._super(model, createViewFunc);
        this.selectedItemsController = selectedItemsController;
        this.availableItemsController = availableItemsController;
    },

    evaluateReadyForExecution: function () {
        if(this.selectedItemsController.items.length > 0) {
            this.view.setExecutionIndicatorReady();
        } else {
            this.view.setExecutionIndicatorNotReady();
        }
    },

    onSelect: function () {
        selectedAvailableItems = this.availableItemsController.getSelectedItems();
        this.selectedItemsController.addItems(selectedAvailableItems);
        this.evaluateReadyForExecution();
    },

    onUnSelect: function () {
        this.selectedItemsController.removeSelected();
        this.evaluateReadyForExecution();
    }
});
