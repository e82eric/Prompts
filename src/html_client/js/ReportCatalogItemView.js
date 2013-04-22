var ReportCatalogItemView = TemplateView.extend({
    init: function (controller){
        this._super(controller, "itemTemplate");
        this.selectElement = this.root.find(".selectable");
    },

    render: function () {
        this.selectElement.click($.proxy(this.onClick,this));

        return this.root;
    },

    onClick: function () {
        this.controller.changeSelect();
    },

    onSelected: function () {
        this.selectElement.attr('class', 'item selectable-selected');
    },

    onUnSelected: function () {
        this.selectElement.attr('class', 'item selectable');
    }
});