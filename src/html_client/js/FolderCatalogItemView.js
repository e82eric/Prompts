var FolderCatalogItemView = TemplateView.extend({
    init: function (controller, childItemsView){
        this._super(controller, "folderItemTemplate");
        this.childItemsView = childItemsView;

        this.expandImage = this.root.find(".expander");

        this.root.append(this.childItemsView.render());
    },

    render: function () {
        this.root.find("div:first").click($.proxy(this.handleClick, this));
        return this.root;
    },

    renderExpand: function () {
        this.expandImage.attr("class", "expander expanded-image");
        $(this.root.children()[1]).show();
    },

    renderCollapse: function () {
        this.expandImage.attr("class", "expander collapsed-image");
        $(this.root.children()[1]).hide();
    },

    handleClick: function (e) {
        this.controller.expanderClicked();
        e.stopPropagation();
    }
});