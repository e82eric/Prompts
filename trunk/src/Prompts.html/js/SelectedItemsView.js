function SelectedItemsView (controller) {
    this.controller = controller;

    this.render = function () {
        this.root = $("<ul class='rootItems'></ul>")
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