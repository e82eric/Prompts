var ReportCatalogItemViewBase = Class.extend({
    init: function (templateName, controller) {
        this._super(controller, templateName);
    },
    
    render: function () {
        return this.root;
    }
});