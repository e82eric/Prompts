var DropDownController = PromptController.extend({

    init: function (model, availableItemsController) {
        this.model = model;
        this.availableItemsController = availableItemsController;
    },

    closePopup: function () {
        this.view.close();
        this.open = false;
    },

    openPopup: function () {
        this.view.open();
        this.open = true;
    },

    onOutsideClick: function () {
        this.closePopup();
    },

    onToggleClick: function () {
        if(this.open) {
            this.closePopup();
        } else {
            this.openPopup();
        }
    },

    onSelection: function(item) {
        this.view.setSelectedItemText(item.model.Label);
        this.closePopup();
    },

    setView: function (val) {
        this.view = val;
        this.closePopup();
    },

    createView: function () {
        this.setView(new DropDownView(this));
        return this.view;
    }
});