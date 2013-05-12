var AvailableItemsController = DisposableItemsController.extend({
    init: function(selector, searchController, itemsDisposer) {
        this.selector = selector;
		this.searchController = searchController;
        this._super(itemsDisposer);
        this.selectedItems = [];
    },
	
	setItems: function(val) {
		this.items = val;
	},

	setDisplayItems: function (val) {
		if(this.displayItems != undefined) {
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
	},

    select: function (shiftKeyPressed, controlKeyPressed, item) {
        this.selectedItems = this.selector.select(shiftKeyPressed, controlKeyPressed, this.displayItems, item);
    },

    getSelectedItems: function () {
        return this.selectedItems;
    },

    setView: function (val) {
    	var ret = this._super(val);
    	this.setDisplayItems(this.items);
    	return ret;
    }
});
