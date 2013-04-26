var MultiSelectPromptController = PromptController.extend({
    init: function (model, availableItemsController, selectedItemsController, promptsController, createViewFunc) {
        this._super(model, promptsController, createViewFunc);
        this.selectedItemsController = selectedItemsController;
        this.availableItemsController = availableItemsController;
    },

    evaluateReadyForExecution: function () {
        return this.selectedItemsController.items.length > 0;
    },

    onSelect: function () {
        selectedAvailableItems = this.availableItemsController.getSelectedItems();
        this.selectedItemsController.addItems(selectedAvailableItems);
        this.setReadyForExecution();
    },

    onUnSelect: function () {
        this.selectedItemsController.removeSelected();
        this.setReadyForExecution();
    },

    selections: function () {
        return _.map(this.selectedItemsController.items, function (item) { return item.model; });
    },
});
