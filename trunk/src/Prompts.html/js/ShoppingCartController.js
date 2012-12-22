function ShoppingCartController (availableItemsController, selectedItemsController) {
    this.availableItemsController = availableItemsController;
    this.selectedItemsController = selectedItemsController;

    this.createView = function () {
        return new ShoppingCartView(this);
    };

    this.onSelect = function () {
        var selectedAvailableItems = availableItemsController.getSelectedItems();
        this.selectedItemsController.addItems(selectedAvailableItems);
    };

    this.onUnSelect = function () {
        this.selectedItemsController.removeSelected();
    }
}