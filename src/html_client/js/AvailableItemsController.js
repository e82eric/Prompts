var AvailableItemsController = Class.extend({
    init: function(selector) {
        this.selector = selector;
    },

    createView: function () {
        this.view = new ItemsView(this, "rootItems");
        return this.view;
    },

    select: function (shiftKeyPressed, controlKeyPressed, item) {
        this.selectedItems = this.selector.select(shiftKeyPressed, controlKeyPressed, this.displayItems, item);
    },

    getSelectedItems: function () {
        return this.selectedItems;
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