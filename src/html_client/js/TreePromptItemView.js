var TreePromptItemView = TemplateView.extend({
    init: function (controller, childItemsLoadingPanelView) {
        this._super(controller, "treePromptItemTemplate");

        this.childItemsLoadingPanelView = childItemsLoadingPanelView;
        this.root.append(this.childItemsLoadingPanelView.render());
        this.expandImage = this.root.find(".expandImage");
        this.selectWrap = this.root.find(".item");
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
        this.selectWrap.attr('class', 'item-selected');
    },

    onUnSelected: function () {
        this.selectWrap.attr('class', 'item');
    },

    deleteItem: function () {
        this.root.remove();
    },

    renderExpand: function () {
        this.expandImage.attr("class", "expandImage expanded-image");
    },

    renderCollapse: function () {
        this.expandImage.attr("class", "expandImage collapsed-image");
    },

    hideExpander: function () {
        this.expandImage.attr("class", "expandImage");
    },

    onRetryClick: function () {
        this.controller.onRetryClick();
    }
});