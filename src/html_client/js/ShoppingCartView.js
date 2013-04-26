var ShoppingCartView = PromptView.extend({
    init: function (controller, templateName){
        this._super(controller, templateName);
        this.availableItems = this.root.find(".available-items");
        this.selectedItems = this.root.find(".selected-items:first");
        this.selectButton = this.root.find(".select-button:first");
        this.unSelectButton = this.root.find(".unselect-button:first");
        this.availableItems.append(this.controller.availableItemsController.createView().render());
        this.selectedItems.append(this.controller.selectedItemsController.createView().render());
    },
    render: function (){
        this.selectButton.click($.proxy(this.onSelectButtonClick, this));
        this.unSelectButton.click($.proxy(this.onUnSelectButtonClick, this));

        return this.root;
    },

    onSelectButtonClick: function () {
        this.controller.onSelect();
    },

    onUnSelectButtonClick: function () {
        this.controller.onUnSelect();
    }
});