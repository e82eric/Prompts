function AvailableItemsView (controller) {
    this.controller = controller;

    this.render = function () {
        var template = $("#availableItemsTemplate").html();
        var templateFunction  = _.template(template);
        var templateHtml = templateFunction(this.controller);
        this.root = $(templateHtml);

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