var EmptyFolderCatalogItemView = CatalogItemView.extend({
    template: $("#emptyFolderItemTemplate").html(),

    render: function () {
        CatalogItemView.prototype.render.apply(this, []);
    }
});