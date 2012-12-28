var LeafTreePromptItemView = Class.extend({
    init: function (controller) {
        this.controller = controller;

        this.root = $("<li class='treeItem' unselectable='on'></li>");
        var template = $("#leafTreePromptItemTemplate").html();

        var templateFunction  = _.template(template);
        var templateHtml = templateFunction(this.controller.model);
        this.root.html(templateHtml);

        this.childrenPanel = this.controller.childPromptItemsLoadingPanel.createView().render();

        this.root.append(this.childrenPanel);

        this.selectWrap = this.root.find("#selectWrap");
        this.selectWrap.addClass('itemTestNotSelected');
    },
    render: function () {
        this.selectWrap.click($.proxy(this.onClick,this));
        return this.root;
    },

    onClick: function (e) {
        this.controller.clicked(e.shiftKey, e.ctrlKey);
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
    }
});