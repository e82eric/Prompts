var FolderCatalogItemView = function (model){
    var base = new CatalogItemView("#folderItemTemplate", model);
    var root = base.render();
    base.render = function () {
        root.find("div:first").click($.proxy(this.handleClick, this));
        this.expandImage = root.find("#ExpandImage:first img");
        this.root = root;
        this.model.changeToggle();
        return root;
    };

    base.renderExpand = function () {
        this.expandImage.attr("src", "../images/tree_expand.png");
        $(this.root.children()[1]).show();
    };

    base.renderCollapse = function () {
        this.expandImage.attr("src", "../images/tree_collapsed.png");
        $(this.root.children()[1]).hide();
    };

    base.handleClick = function (e) {
        this.model.changeToggle();
        e.stopPropagation();
    };

    return base;
};