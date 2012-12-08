var CatalogItemView = function (templateName, model) {
    this.templateName = templateName;
    this.model = model;

    this.onSelected = function () {
        this.selectElement.attr('class', 'selectedGlow');
    };

    this.onUnSelected = function () {
        this.selectElement.attr('class', 'catalogItem');
    };

    this.render = function () {
        this.root = $("<li></li>");

        var template = $(this.templateName).html();

        var t = _.template(template);
        var h = t(this.model.model);
        this.root.html(h);
        this.root.attr("class", "ReportView");

        this.hoverElement = this.root.find("#hoverWrap");
        this.selectElement = this.root.find("#selectWrap:first");

        this.hoverElement.mouseover($.proxy(this.onMouseOver,this));
        this.hoverElement.mouseleave($.proxy(this.onMouseLeave,this));
        this.hoverElement.click($.proxy(this.onClick,this));

        var childCatalogItems = this.model.Children;

        if (!(childCatalogItems.length == 0)) {
            var childCatalogView = new CatalogItemsView(childCatalogItems, "childItems");
            this.root.append(childCatalogView.render());
        }

        return this.root;
    };

    this.onMouseOver = function () {
        this.hoverElement.attr('class', 'hoverGlow');
    };

    this.onMouseLeave = function () {
        this.hoverElement.attr('class', 'catalogItem');
    };

    this.onClick = function () {
        this.model.changeSelect();
    };
};