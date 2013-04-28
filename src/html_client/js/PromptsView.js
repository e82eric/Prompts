var PromptsView = TemplateView.extend({
    init: function(controller) {
        this._super(controller, "promptsTemplate");
        this.ul = this.root.find("ul");
    },

    render: function () {
        return this.root;
    },

    renderItems: function(items) {
        _.each(
            items,
            function (item) {
                var itemView = item.createView();
                this.ul.append(itemView.render());
            },
            this);
    }
});