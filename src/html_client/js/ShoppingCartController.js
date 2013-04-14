var ShoppingCartController = PromptController.extend({
    init: function (availableItemsController, selectedItemsController) {
        this.availableItemsController = availableItemsController;
        this.selectedItemsController = selectedItemsController;
    },

    createView: function () {
        return new ShoppingCartView(this);
    },

    onSelect: function () {
        var selectedAvailableItems = this.availableItemsController.getSelectedItems();
        this.selectedItemsController.addItems(selectedAvailableItems);
    },

    onUnSelect: function () {
        this.selectedItemsController.removeSelected();
    }
});