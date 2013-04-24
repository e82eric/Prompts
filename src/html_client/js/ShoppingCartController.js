var ShoppingCartController = PromptController.extend({
    init: function (model, availableItemsController, selectedItemsController) {
        this.model = model;
        this.availableItemsController = availableItemsController;
        this.selectedItemsController = selectedItemsController;
    },

    createView: function () {
        this.view = new ShoppingCartView(this);
        return this.view;
    },

    onSelect: function () {
        var selectedAvailableItems = this.availableItemsController.getSelectedItems();
        this.selectedItemsController.addItems(selectedAvailableItems);
    },

    onUnSelect: function () {
        this.selectedItemsController.removeSelected();
    }
});