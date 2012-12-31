var FolderCatalogItemView = Class.extend({
    init: function (controller, childItemsView){
        this.controller = controller;
        this.childItemsView = childItemsView;

        var base = new ReportCatalogItemViewBase("#folderItemTemplate", controller);
        this.root = base.render();

        this.expandImage = this.root.find("#ExpandImage:first img");

        this.root.append(this.childItemsView.render());
    },

    render: function () {
        this.root.find("div:first").click($.proxy(this.handleClick, this));
        return this.root;
    },

    renderExpand: function () {
        this.expandImage.attr("src", "../images/tree_expand.png");
        $(this.root.children()[1]).show();
    },

    renderCollapse: function () {
        this.expandImage.attr("src", "../images/tree_collapsed.png");
        $(this.root.children()[1]).hide();
    },

    handleClick: function (e) {
        this.controller.expanderClicked();
        e.stopPropagation();
    }
});