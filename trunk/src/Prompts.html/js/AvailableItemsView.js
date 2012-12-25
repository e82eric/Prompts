function AvailableItemsView (controller) {
    this.controller = controller;

    this.render = function () {
        this.root = $("<ul class='rootItems'></ul>");

        var result = [];
        _.each(
            this.controller.items,
            function (controller) {
                var itemView = controller.createView();
                this.root.append(itemView.render());
            },
            this
        );

        return this.root;
    };

    this.renderItems = function (controllers) {
        _.each(
            controllers,
            function (controller) {
                var itemView = controller.createView();
                this.root.append(itemView.render());
            },
            this
        );
    }
}