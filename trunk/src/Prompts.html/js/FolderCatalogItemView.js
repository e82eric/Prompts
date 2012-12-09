var FolderCatalogItemView = function (controller){
    this.controller = controller;

    this.render = function () {
        var base = new CatalogItemViewBase("#folderItemTemplate", controller);
        this.root = base.render();

        this.root.find("div:first").click($.proxy(this.handleClick, this));
        this.expandImage = this.root.find("#ExpandImage:first img");

        var childCatalogItems = this.controller.Children;

        if (!(childCatalogItems.length == 0)) {
            var childCatalogView = new CatalogItemsView(childCatalogItems, "childItems");
            this.root.append(childCatalogView.render());
        }

        this.controller.changeToggle();
        return this.root;
    };

    this.renderExpand = function () {
        this.expandImage.attr("src", "../images/tree_expand.png");
        $(this.root.children()[1]).show();
    };

    this.renderCollapse = function () {
        this.expandImage.attr("src", "../images/tree_collapsed.png");
        $(this.root.children()[1]).hide();
    };

    this.handleClick = function (e) {
        this.controller.changeToggle();
        e.stopPropagation();
    };
};