function DropDownController (availableItemsController) {
    this.availableItemsController = availableItemsController;

    this.closePopup = function () {
        this.view.close();
        this.open = false;
    };

    this.openPopup = function () {
        this.view.open();
        this.open = true;
    };

    this.onOutsideClick = function () {
        this.closePopup();
    };

    this.onToggleClick = function () {
        if(this.open) {
            this.closePopup();
        } else {
            this.openPopup();
        }
    };

    this.onSelection = function(item) {
        this.view.setSelectedItemText(item.model.Label);
        this.closePopup();
    };

    this.setView = function (val) {
        this.view = val;
        this.closePopup();
    };

    this.createView = function () {
        this.setView(new DropDownView(this));
        return this.view;
    }
}