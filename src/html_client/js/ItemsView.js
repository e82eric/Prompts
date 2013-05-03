var ItemsView = Class.extend({
    init: function(controller, listClass){
        this.controller = controller;
        this.listClass = listClass;
        this.root = $("<ul></ul>");
    },

    render: function () {
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