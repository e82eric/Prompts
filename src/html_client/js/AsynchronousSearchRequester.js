var AsynchronousSearchRequester = Class.extend({
    init: function (searchStringParser, repository, promptItemControllersBuilder) {
        this.searchStringParser = searchStringParser;
        this.repository = repository;
        this._promptItemControllersBuilder = promptItemControllersBuilder;
    },

    execute: function (searchString, availableItemsController) {
        var self = this;
        this.lastSearchString = searchString;
        var search = this.searchStringParser.parse(searchString);

        this.repository.get(search.childItemsRequest, function (model) {
            var controllers = self._promptItemControllersBuilder.build(model.AvailableItems);
            var searchResults = new Search(search).execute(controllers);
            availableItemsController.setItems(searchResults);
        });
    },

    executeLastRequest: function (availableItemsController) {
        var search = this.searchStringParser.parse(this.lastSearchString);

        this.repository.get(search.childItemsRequest, function (model) {
            var controllers = [];

            _.each(
                model.AvailableItems,
                function (availableItem) {
                    var controller = new PromptItemController(availableItem, availableItemsController);
                    controllers.push(controller);
                }
            );

            var searchResults = new Search(search).execute(controllers);
            availableItemsController.setItems(searchResults);
        });
    },
});