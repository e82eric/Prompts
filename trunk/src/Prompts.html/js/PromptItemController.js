function PromptItemController (model, availableItemsController) {
    this.model = model;
    this.availableItemsController = availableItemsController;

    this.Select = function () {
        this.view.onSelected();
    }

    this.UnSelect = function () {
        this.view.onUnSelected();
    }

    this.clicked = function () {
        this.availableItemsController.select(this);
    }

    this.createView = function () {
        this.view = new PromptItemView(this);
        return this.view;
    }
}