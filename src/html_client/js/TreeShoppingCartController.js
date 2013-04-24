var TreeShoppingCartController = PromptController.extend({
    init: function (model, availableItemsController, selectedItemsController) {
        this.model = model;
        this.availableItemsController = availableItemsController;
        this.selectedItemsController = selectedItemsController;
    },

    createView: function () {
        this.view = new TreeShoppingCartView(this);
        return this.view;
    },

    onSelect: function () {
        selectedAvailableItems = this.availableItemsController.getSelectedItems();
        this.selectedItemsController.addItems(selectedAvailableItems);
    },

    onUnSelect: function () {
        this.selectedItemsController.removeSelected();
    }
});