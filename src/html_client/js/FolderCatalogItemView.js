var FolderCatalogItemView = TemplateView.extend({
    init: function (controller, childItemsView){
        this._super(controller, "folderItemTemplate");
        this.childItemsView = childItemsView;

        this.expandImage = this.root.find(".expandImage");

        this.root.append(this.childItemsView.render());
    },

    render: function () {
        this.root.find("div:first").click($.proxy(this.handleClick, this));
        return this.root;
    },

    renderExpand: function () {
        this.expandImage.attr("class", "expandImage expanded-image");
        $(this.root.children()[1]).show();
    },

    renderCollapse: function () {
        this.expandImage.attr("class", "expandImage collapsed-image");
        $(this.root.children()[1]).hide();
    },

    handleClick: function (e) {
        this.controller.expanderClicked();
        e.stopPropagation();
    }
});