var ClientSideSearch = Class.extend({
	init: function (searchStringParser) {
		this.searchStringParser = searchStringParser;
	},

	execute: function (searchString, availableItemsController) {
        var itemSearch = this.searchStringParser.parse(searchString);
        var itemsSearch = new Search(itemSearch);
        var searchResults = itemsSearch.execute(availableItemsController.items);
        availableItemsController.setDisplayItems(searchResults);
	}
});