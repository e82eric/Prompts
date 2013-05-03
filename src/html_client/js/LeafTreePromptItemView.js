var LeafTreePromptItemView = TemplateView.extend({
    init: function (controller) {
        this._super(controller, "treePromptItemTemplate");
        this.selectWrap = this.root.find(".selectable");
        this.root.find(".expander").attr("class", "expander spacer-image");
    },

    render: function () {
        this.selectWrap.click($.proxy(this.onClick,this));
        return this.root;
    },

    onClick: function (e) {
        this.controller.clicked(e.shiftKey, e.ctrlKey);
    },

    onSelected: function () {
        this.selectWrap.attr('class', 'item selectable-selected');
    },

    onUnSelected: function () {
        this.selectWrap.attr('class', 'item selectable');
    }
});