var ShoppingCartView = PromptView.extend({
    init: function (controller) {
        this._super(controller, "shoppingCartTemplate");
        this.selectElement = this.root.find("#selectButton:first");
        this.availableItems = this.root.find("#availableItems");
        this.selectedItems = this.root.find("#selectedItems:first");
        this.selectButton = this.root.find("#selectButton:first");
        this.unSelectButton = this.root.find("#unSelectButton:first");
        this.searchStringInput = this.root.find("#searchString:first");
        this.searchButton = this.root.find("#searchButton:first");
        this.availableItems.append(this.controller.availableItemsController.createView().render());
        this.selectedItems.append(this.controller.selectedItemsController.createView().render());
    },

    render: function (){
        this.selectElement.click($.proxy(this.onSelect, this));
        this.selectButton.click($.proxy(this.onSelectButtonClick, this));
        this.unSelectButton.click($.proxy(this.onUnSelectButtonClick, this));
        this.searchButton.click($.proxy(this.onSearch, this));
        return this.root;
    },

    onSelectButtonClick: function () {
        this.controller.onSelect();
    },

    onUnSelectButtonClick: function () {
        this.controller.onUnSelect();
    },

    onSearch: function () {
        this.controller.availableItemsController.search(this.searchStringInput.val());
    }
});