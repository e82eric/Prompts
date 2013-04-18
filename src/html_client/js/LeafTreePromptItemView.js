var LeafTreePromptItemView = TemplateView.extend({
    init: function (controller) {
        this._super(controller, "leafTreePromptItemTemplate");
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
    }
});