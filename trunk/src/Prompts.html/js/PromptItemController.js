function PromptItemController (model, availableItemsController) {
    this.model = model;
    this.availableItemsController = availableItemsController;
    this.isSelected = false;

    this.Select = function () {
        this.view.onSelected();
        this.isSelected = true;
    };

    this.UnSelect = function () {
        this.view.onUnSelected();
        this.isSelected = false;
    };

    this.clicked = function (shiftKeyPressed, controlKeyPressed) {
        this.availableItemsController.select(shiftKeyPressed, controlKeyPressed, this);
    };

    this.createView = function () {
        var view = new PromptItemView(this);
        this.setView(view);
        return this.view;
    };

    this.setView = function (val) {
        this.view = val;
    };

    this.deleteItem = function () {
        this.view.deleteItem();
    }
}