var PromptsView = Class.extend({
    init: function() {
        this.root = $("<ul></ul>");
    },

    render: function () {
        return this.root;
    },

    renderItems: function(items) {
        _.each(
            items,
            function (item) {
                var itemView = item.createView();
                this.root.append(itemView.render());
            },
            this);
    }
});