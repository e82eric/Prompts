var PromptsRequester = Class.extend({
    init: function (repository, builder) {
       this.repository = repository;
       this.builder = builder;
    },

    execute: function (request, itemsController) {
        this.lastRequest = request;

        this.repository.get(
            request,
            $.proxy(
                function(result) {
                    var controllers = this.builder.build(result);
                    itemsController.setItems(controllers);
                },
                this
            )
        );
    },

    executeLastRequest: function (itemsController) {
        this.repository.get(
            this.lastRequest,
            $.proxy(
                function(result) {
                    var controllers = this.builder.build(result);
                    itemsController.setItems(controllers);
                },
                this
            )
        );
    }
});