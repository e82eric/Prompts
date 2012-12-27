var TreePromptItemView = Class.extend({
    init: function (controller) {
        this.controller = controller;

        this.root = $("<li unselectable='on'></li>");
        var template = $("#treePromptItemTemplate").html();

        var templateFunction  = _.template(template);
        var templateHtml = templateFunction(this.controller.model);
        this.root.html(templateHtml);

        this.childrenPanel = this.controller.childPromptItemsLoadingPanel.createView().render();

        this.root.append(this.childrenPanel);

        this.expandImage = this.root.find("#expandImage");
        this.selectWrap = this.root.find("#selectWrap");
        this.selectWrap.addClass('itemTestNotSelected');
    },
    render: function () {
        this.expandImage.click($.proxy(this.onExpandClick,this));
        this.selectWrap.click($.proxy(this.onClick,this));
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
        this.childrenPanel.show();
    },

    renderCollapse: function () {
        this.expandImage.attr("src", "../images/tree_collapsed.png");
        this.childrenPanel.hide();
    }
});