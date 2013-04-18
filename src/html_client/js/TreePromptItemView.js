var TreePromptItemView = TemplateView.extend({
    init: function (controller, childItemsLoadingPanelView) {
        this._super(controller, "treePromptItemTemplate");

        this.childItemsLoadingPanelView = childItemsLoadingPanelView;
        this.root.append(this.childItemsLoadingPanelView.render());
        this.expandImage = this.root.find("#expandImage");
        this.selectWrap = this.root.find("#selectWrap");
        this.selectWrap.addClass('itemTestNotSelected');
    },
    render: function () {
        this.expandImage.click($.proxy(this.onExpandClick,this));
        this.selectWrap.click($.proxy(this.onClick,this));
        this.childItemsLoadingPanelView.root.find("#retry").click($.proxy(this.onRetryClick, this));

        return this.root;
    },

    onClick: function (e) {
        this.controller.clicked(e.shiftKey, e.ctrlKey);
    },

    onExpandClick: function () {
        this.controller.expanderClicked();
    },

    onSelected: function () {
        this.selectWrap.addClass('itemTestSelect');
        this.selectWrap.removeClass('itemTestNotSelected');
    },

    onUnSelected: function () {
        this.selectWrap.removeClass('itemTestSelect');
        this.selectWrap.addClass('itemTestNotSelected');
    },

    deleteItem: function () {
        this.root.remove();
    },

    renderExpand: function () {
        this.expandImage.attr("src", "../images/tree_expand.png");
        this.childItemsLoadingPanelView.root.show();
    },

    renderCollapse: function () {
        this.expandImage.attr("src", "../images/tree_collapsed.png");
        this.childItemsLoadingPanelView.root.hide();
    },

    hideExpander: function () {
        this.expandImage.hide();
    },

    onRetryClick: function () {
        this.controller.onRetryClick();
    }
});