var LeafTreePromptItemView = TemplateView.extend({
    init: function (controller) {
        this._super(controller, "treePromptItemTemplate");
        this.selectWrap = this.root.find(".item");
    },

    render: function () {
        this.selectWrap.click($.proxy(this.onClick,this));
        return this.root;
    },

    onClick: function (e) {
        this.controller.clicked(e.shiftKey, e.ctrlKey);
    },

    onSelected: function () {
        this.selectWrap.attr('class', 'item-selected');
    },

    onUnSelected: function () {
        this.selectWrap.attr('class', 'item');
    }
});