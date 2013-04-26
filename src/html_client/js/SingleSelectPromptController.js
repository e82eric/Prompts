var SingleSelectPromptController = PromptController.extend({
	init: function (model, availableItemsController, promptsController, createViewFunc) {
        this._super(model, promptsController, createViewFunc);
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

	evaluateReadyForExecution: function () {
        return this.selectedItem != null;
    },

    onSelection: function(item) {
        this.selectedItem = item;
        this.view.setSelectedItemText(item.model.Label);
        this.closePopup();

        this.setReadyForExecution();
    },

    selections: function () {
        return [this.selectedItem.model];
    },

    setView: function (val) {
    	this._super(val);
        this.closePopup();
    }
});