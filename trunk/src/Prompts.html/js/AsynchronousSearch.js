var AsynchronousSearch = Class.extend({
    init: function (searchStringParser, repository) {
        this.searchStringParser = searchStringParser;
        this.repository = repository;
    },

    execute: function (searchString, availableItemsController) {
        var search = this.searchStringParser.parse(searchString);

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
    }
});