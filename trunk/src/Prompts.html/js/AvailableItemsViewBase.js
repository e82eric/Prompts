var AvailableItemsViewBase = Class.extend({
    init: function(controller, listClass){
        this.controller = controller;
        this.listClass = listClass;
    },

    render: function () {
        this.root = $("<ul class='" + this.listClass + "'></ul>");

        _.each(
            this.controller.items,
            function (controller) {
                var itemView = controller.createView();
                this.root.append(itemView.render());
            },
            this
        );

        return this.root;
    },

    renderItems: function (controllers) {
        _.each(
            controllers,
            function (controller) {
                var itemView = controller.createView();
                this.root.append(itemView.render());
            },
            this
        );
    }
});