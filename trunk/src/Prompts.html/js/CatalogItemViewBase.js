var CatalogItemViewBase = function (templateName, controller) {
    this.templateName = templateName;
    this.controller = controller;

    this.render = function () {
        this.root = $("<li></li>");

        var template = $(this.templateName).html();

        var t = _.template(template);
        var h = t(this.controller.model);
        this.root.html(h);
        this.root.attr("class", "ReportView");

        return this.root;
    };
};