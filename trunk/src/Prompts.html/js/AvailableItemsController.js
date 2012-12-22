function AvailableItemsController (selector, promptItemControllersProvider) {
    this.selector = selector;
    this.promptItemControllersProvider = promptItemControllersProvider;

    this.createView = function () {
        this.view = new AvailableItemsView(this);
        return this.view;
    };

    this.select = function (shiftKeyPressed, controlKeyPressed, item) {
        this.selector.select(shiftKeyPressed, controlKeyPressed, this.items, item);
    };

    this.setItems = function (val) {
        this.items = val;
    };

    this.getSelectedItems = function () {
        var models = [];

        _.each(
            this.items,
            function (item) {
                if(item.isSelected) {
                    models.push(item.model);
                }
            },
            this
        )

        return this.promptItemControllersProvider.get(models);
    };

    this.search = function (searchString) {
        var parser = new SearchStringParser();
        var itemSearch = parser.parse(searchString);
        var itemsSearch = new Search(itemSearch);

        _.each(
            this.items,
            function (item) {
                item.deleteItem();
            },
            this
        )

        var searchResults = itemsSearch.execute(this.items);

        this.view.renderItems(searchResults);
    }
}