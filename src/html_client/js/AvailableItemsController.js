var AvailableItemsController = SelectableItemsController.extend({
    init: function(selector, searchController, itemsDisposer) {
		this.searchController = searchController;
        this._super(selector, itemsDisposer);
        this.selectedItems = [];
    },

	setDisplayItems: function (val) {
		if(this.displayItems == undefined) {
			this.itemsDisposer.dispose(this.items);
		} else {
			this.itemsDisposer.dispose(this.displayItems);
		}
		this.displayItems = val;
		this.view.renderItems(this.displayItems);
	},
		
	setSearchString: function (val) {
		this.searchString = val;
	},

	search: function () {
		this.searchController.execute(this.searchString, this);
	}
});
