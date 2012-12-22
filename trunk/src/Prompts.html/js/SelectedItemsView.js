function SelectedItemsView (controller) {
    this.controller = controller;

    this.render = function () {
        var template = $("#availableItemsTemplate").html();

        var templateFunction  = _.template(template);
        var templateHtml = templateFunction(this.controller);
        this.root = $(templateHtml);

        return this.root;
    }

    this.addItems = function(controllers) {
        _.each(
            controllers,
            function (item) {
                var itemView = item.createView();
                this.root.append(itemView.render());
            },
            this
        )
    }
}