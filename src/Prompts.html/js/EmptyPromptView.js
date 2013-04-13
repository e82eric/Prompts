var EmptyPromptView = PromptView.extend({
    init: function (controller) {
        this.controller = controller;

        this.root = $("<div></div>");
        var template = $("#emptyPromptTemplate").html();

        var templateFunction  = _.template(template);
        var templateHtml = templateFunction(this.controller.model);
        this.root.html(templateHtml);
    },
    render: function () {
        return this.root;
    }
});