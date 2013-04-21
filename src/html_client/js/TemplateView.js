var TemplateView = Class.extend({
    init: function (controller, templateId) {
        this.controller = controller;

        var template = _.template($("#" + templateId).text());
        var templateHtml = template(this.controller);
        this.root = $(templateHtml);
    }, 
    
    render: function () {
        return this.root;
    }
});