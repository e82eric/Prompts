var SingleSelectPromptController = PromptController.extend({
	init: function (model, availableItemsController, createViewFunc) {
        this._super(model, createViewFunc);
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
        if(this.selectedItem != null) {
            this.view.setExecutionIndicatorReady();
        } else {
            this.view.setExecutionIndicatorNotReady();
        }
    },

    onSelection: function(item) {
        this.selectedItem = item;
        this.view.setSelectedItemText(item.model.Label);
        this.closePopup();

        this.evaluateReadyForExecution();
    },

    setView: function (val) {
    	this._super(val);
        this.closePopup();
    }
});