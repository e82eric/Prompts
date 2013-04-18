var TreeShoppingCartView = PromptView.extend({
    init: function (controller){
        this._super(controller, "treeShoppingCartTemplate");

        this.selectElement = this.root.find("#selectButton:first");
        this.selectElement.click($.proxy(this.onSelect,this));

        this.availableItems = this.root.find("#availableItems");
        this.selectedItems = this.root.find("#selectedItems:first");
        this.selectButton = this.root.find("#selectButton:first");
        this.unSelectButton = this.root.find("#unSelectButton:first");
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