var PromptItemControllerBuilder = Class.extend({
	build:function (params) {
		return new PromptItemController(params.model, this.availableItemsController);
	},

	setAvailableItemsController: function (val) {
		this.availableItemsController = val;
	}	
});
