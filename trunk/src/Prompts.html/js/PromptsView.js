var PromptsView = function(items, css){
    this.items = items;
    this.css = css;

    this.render = function () {
        this.root = $("<ul class='rootItems'></ul>");

        this.root.attr("class", css);

        _.each(
            this.items,
            function (item) {
                this.renderItem(item);
            },
            this);

        return this.root;
    };

    this.renderItem = function (item) {
        var itemView = item.createView();
        this.root.append(itemView.render());
    };
};