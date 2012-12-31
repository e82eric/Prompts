var PromptsRequester = Class.extend({
    init: function (repository, builder) {
       this.repository = repository;
       this.builder = builder;
    },

    execute: function (request, itemsController) {
        this.repository.get(
            request,
            $.proxy(
                function(result) {
                    var controllers = this.builder.build(result);
                    itemsController.setItems(controllers);
                },
                this
            )
        )
    }
});