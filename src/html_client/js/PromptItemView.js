var PromptItemView = TemplateView.extend({
    init: function (controller) {
        this.controller = controller;
        this._super(controller, "promptItemTemplate");
        this.selectElement = this.root.find(".selectable");
    },
    
    render: function () {
        this.root.click($.proxy(this.onClick,this));
        return this.root;
    },

    onClick: function (e) {
        this.controller.clicked(e.shiftKey, e.ctrlKey);
    },

    onSelected: function () {
        this.selectElement.attr('class', 'item selectable-selected');
    },

    onUnSelected: function () {
        this.selectElement.attr('class', 'item selectable');
    },

    deleteItem: function () {
        this.root.remove();
    }
});