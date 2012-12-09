var CatalogItemsView = function(items, css){
    this.items = items;
    this.css = css;

    this.render = function () {
        this.root = $("<ul></ul>");

        this.root.attr("class", css);

        _.each(items, function (catalogItem) {
            this.renderCatalogItem(catalogItem);
        }, this);

        return this.root;
    };

    this.renderCatalogItem = function (catalogItem) {
        var catalogItemView = catalogItem.CreateView();
        var childRender = catalogItemView.render();
        this.root.append(childRender);
    };
};