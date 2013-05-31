var ShoppingCartView = PromptView.extend({
    init: function (controller, templateName){
        this._super(controller, templateName);
        this.availableItems = this.root.find(".available-items");
        this.selectedItems = this.root.find(".selected-items:first");
        this.selectButton = this.root.find(".select-button > button");
        this.unSelectButton = this.root.find(".unselect-button > button");
		this.availableItemsView = this.controller.availableItemsController.createView(function (controller) { return new ItemsView(controller, "rootItems"); } )
		this.selectedItemsView = this.controller.selectedItemsController.createView();
    },

    render: function (){
		this.selectedItems.append(this.selectedItemsView.render());
		this.availableItems.append(this.availableItemsView.render());

        this.selectButton.click($.proxy(this.onSelectButtonClick, this));
        this.unSelectButton.click($.proxy(this.onUnSelectButtonClick, this));

        return this.root;
    },

	renderSmall: function () {
		this.smallRoot.find(".small-prompt-content").append(this.selectedItemsView.render());
		return this._super();
	},

    onSelectButtonClick: function () {
        this.controller.onSelect();
    },

    onUnSelectButtonClick: function () {
        this.controller.onUnSelect();
    },

	showSmall: function () {
		this.smallRoot.find(".small-prompt-content").append(this.selectedItemsView.render());
		this._super();
	}
});
