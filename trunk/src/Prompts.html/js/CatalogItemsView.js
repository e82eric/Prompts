var CatalogItemsView = function(items, css){
    var root = $("<ul></ul>");

    return {
        root: root,
        items: items,
        css: css,
        tagName: "ul",

        render: function () {
            root.attr("class", css);

            _.each(items, function (catalogItem) {
                this.renderCatalogItem(catalogItem);
            }, this);

            return root;
        },

        renderCatalogItem: function (catalogItem) {
            var catalogItemView = catalogItem.CreateView();
            var childRender = catalogItemView.render();
            root.append(childRender);
        }
    }
};