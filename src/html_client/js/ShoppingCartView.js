var ShoppingCartView = PromptView.extend({
    init: function (controller) {
        this._super(controller, "shoppingCartTemplate");
        this.availableItems = this.root.find(".available-items");
        this.selectedItems = this.root.find(".selected-items:first");
        this.selectButton = this.root.find(".select-button:first");
        this.unSelectButton = this.root.find(".unselect-button:first");
        this.searchStringInput = this.root.find(".search-input:first");
        this.searchButton = this.root.find(".search-button:first");
        this.availableItems.append(this.controller.availableItemsController.createView().render());
        this.selectedItems.append(this.controller.selectedItemsController.createView().render());
    },

    render: function (){
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