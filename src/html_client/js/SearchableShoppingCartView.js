var SearchableShoppingCartView = ShoppingCartView.extend({
    init: function (controller) {
        this._super(controller, "shoppingCartTemplate");
        this.searchStringInput = this.root.find(".search-input");
        this.searchButton = this.root.find(".search-button");
    },

    render: function (){
        this.searchStringInput.keyup($.proxy(this.onSearchStringChange, this));
		this.searchButton.click($.proxy(this.onSearch, this));
        return this._super();
    },

    onSearch: function () {
        this.controller.availableItemsController.search(this.searchStringInput.val());
    },

	onSearchStringChange: function (val) {
		this.controller.availableItemsController.setSearchString(this.searchStringInput.val());
	}
});
