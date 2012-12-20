function PromptItemView (controller) {
    this.controller = controller;

    this.render = function () {
        this.root = $("<li unselectable='on'></li>");
        var template = $("#promptItemTemplate").html();

        var templateFunction  = _.template(template);
        var templateHtml = templateFunction(this.controller.model);
        this.root.html(templateHtml);

        this.root.attr("class", "ReportView");

        this.hoverElement = this.root.find("#hoverWrap");
        this.selectElement = this.root.find("#selectWrap:first");

        this.hoverElement.mouseover($.proxy(this.onMouseOver,this));
        this.hoverElement.mouseleave($.proxy(this.onMouseLeave,this));
        this.hoverElement.click($.proxy(this.onClick,this));

        return this.root;
    }

    this.onMouseOver = function () {
        this.hoverElement.attr('class', 'hoverGlow');
    };

    this.onMouseLeave = function () {
        this.hoverElement.attr('class', 'catalogItem');
    };

    this.onClick = function (e) {
        this.controller.clicked(e.shiftKey, e.ctrlKey);
    };

    this.onSelected = function () {
        this.selectElement.attr('class', 'selectedGlow');
    };

    this.onUnSelected = function () {
        this.selectElement.attr('class', 'catalogItem');
    };
}