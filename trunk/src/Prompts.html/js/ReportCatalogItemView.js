var ReportCatalogItemView = function (controller){
    this.controller = controller;

    this.render = function () {
        var base = new CatalogItemViewBase("#itemTemplate", this.controller);
        this.root = base.render();

        this.hoverElement = this.root.find("#hoverWrap");
        this.selectElement = this.root.find("#selectWrap:first");

        this.hoverElement.mouseover($.proxy(this.onMouseOver,this));
        this.hoverElement.mouseleave($.proxy(this.onMouseLeave,this));
        this.hoverElement.click($.proxy(this.onClick,this));

        return this.root;
    };

    this.onMouseOver = function () {
        this.hoverElement.attr('class', 'hoverGlow');
    };

    this.onMouseLeave = function () {
        this.hoverElement.attr('class', 'catalogItem');
    };

    this.onClick = function () {
        this.controller.changeSelect();
    };

    this.onSelected = function () {
        this.selectElement.attr('class', 'selectedGlow');
    };

    this.onUnSelected = function () {
        this.selectElement.attr('class', 'catalogItem');
    };
};