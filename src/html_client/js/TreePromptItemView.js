var TreePromptItemView = TemplateView.extend({
    init: function (controller, childItemsLoadingPanelView) {
        this._super(controller, "treePromptItemTemplate");

        this.childItemsLoadingPanelView = childItemsLoadingPanelView;
        this.root.append(this.childItemsLoadingPanelView.render());
        this.expandImage = this.root.find(".expander");
        this.selectElement = this.root.find(".selectable");
    },
    render: function () {
        this.expandImage.click($.proxy(this.onExpandClick,this));
        this.selectElement.click($.proxy(this.onClick,this));
        this.childItemsLoadingPanelView.root.find(".retry").click($.proxy(this.onRetryClick, this));

        return this.root;
    },

    onClick: function (e) {
        this.controller.clicked(e.shiftKey, e.ctrlKey);
    },

    onExpandClick: function () {
        this.controller.expanderClicked();
    },

    onSelected: function () {
        this.selectElement.attr('class', 'item selectable-selected');
    },

    onUnSelected: function () {
        this.selectElement.attr('class', 'item selectable');
    },

    deleteItem: function () {
        this.root.remove();
    },

    renderExpand: function () {
        this.expandImage.attr("class", "expander expanded-image");
    },

    renderCollapse: function () {
        this.expandImage.attr("class", "expander collapsed-image");
    },

    hideExpander: function () {
        this.expandImage.attr("class", "expander spacer-image");
    },

    onRetryClick: function () {
        this.controller.onRetryClick();
    }
});