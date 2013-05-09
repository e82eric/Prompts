var ReportCatalogRequester = Class.extend({
    init: function(repository, builder) {
        this.repository = repository;
        this.builder = builder;
    },
    execute: function (availableItemsController) {
        this.repository.get(
            {},
            $.proxy(
                function (models) {
                    var controllers = this.builder.build(models);
                    availableItemsController.setItems(controllers);
            },
            this));
    }
});