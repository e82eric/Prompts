var SearchableShoppingCartView = ShoppingCartView.extend({
    init: function (controller) {
        this._super(controller, "shoppingCartTemplate");
        this.searchStringInput = this.root.find(".search-input:first");
        this.searchButton = this.root.find(".search-button:first");
    },

    render: function (){
        this.searchButton.click($.proxy(this.onSearch, this));
        return this._super();
    },

    onSearch: function () {
        this.controller.availableItemsController.search(this.searchStringInput.val());
    }
});