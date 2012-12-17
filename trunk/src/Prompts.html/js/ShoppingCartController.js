function ShoppingCartController (availableItemsController) {
    this.availableItemsController = availableItemsController;

    this.createView = function () {
        return new ShoppingCartView(this);
    }
}