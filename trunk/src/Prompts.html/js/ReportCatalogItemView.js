var ReportCatalogItemView = Class.extend({
    init: function (controller){
        this.controller = controller;
        var base = new ReportCatalogItemViewBase("#itemTemplate", this.controller);
        this.root = base.render();

        this.hoverElement = this.root.find("#hoverWrap");
        this.selectElement = this.root.find("#selectWrap:first");
    },

    render: function () {
        this.hoverElement.mouseover($.proxy(this.onMouseOver,this));
        this.hoverElement.mouseleave($.proxy(this.onMouseLeave,this));
        this.hoverElement.click($.proxy(this.onClick,this));

        return this.root;
    },

    onMouseOver: function () {
        this.hoverElement.attr('class', 'hoverGlow');
    },

    onMouseLeave: function () {
        this.hoverElement.attr('class', 'catalogItem');
    },

    onClick: function () {
        this.controller.changeSelect();
    },

    onSelected: function () {
        this.selectElement.attr('class', 'selectedGlow');
    },

    onUnSelected: function () {
        this.selectElement.attr('class', 'catalogItem');
    }
});