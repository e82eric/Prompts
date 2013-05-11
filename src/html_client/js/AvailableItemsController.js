var AvailableItemsController = AsynchronousSearchAvailableItemsController.extend({
    init: function(selector) {
        this._super(selector);
    },

    createView: function () {
        this.view = new ItemsView(this, "rootItems");
        return this.view;
    },

    search: function (searchString) {
        var parser = new SearchStringParser();
        var itemSearch = parser.parse(searchString);
        var itemsSearch = new Search(itemSearch);

        _.each(
            this.items,
            function (item) {
                item.deleteItem();
            },
            this
        );

        var searchResults = itemsSearch.execute(this.items);

        this.view.renderItems(searchResults);
        this.displayItems = searchResults;
    },

    setItems: function (val) {
        this.items = val;
        this.displayItems = this.items;
    }
});