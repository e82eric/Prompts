function DropDownController (availableItemsController) {
    this.availableItemsController = availableItemsController;

    this.close = function () {
        this.view.close();
        this.open = false;
    }

    this.open = function () {
        this.view.open();
        this.open = true;
    }

    this.onOutsideClick = function () {
        this.close();
    }

    this.onToggleClick = function () {
        if(this.open) {
            this.close();
        } else {
            this.open();
        }
    };

    this.onSelection = function(item) {
        this.view.setSelectedItemText(item.model.Label);
        this.close();
    }

    this.setView = function (val) {
        this.view = val;
        this.close();
    }

    this.createView = function () {
        this.setView(new DropDownView(this));
        return this.view;
    }
}