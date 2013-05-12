var ItemsView = Class.extend({
    init: function(controller, listClass){
        this.controller = controller;
        this.listClass = listClass;
        this.root = $("<ul></ul>");
    },

    render: function () {
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