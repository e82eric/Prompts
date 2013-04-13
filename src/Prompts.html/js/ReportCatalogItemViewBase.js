var ReportCatalogItemViewBase = Class.extend({
    init: function (templateName, controller) {
        this.templateName = templateName;
        this.controller = controller;

        this.root = $("<li></li>");

        var template = $(this.templateName).html();

        var t = _.template(template);
        var h = t(this.controller.model);
        this.root.html(h);
        this.root.attr("class", "ReportView");
    },
    render: function () {
        return this.root;
    }
});