var PromptView = TemplateView.extend({
	init: function (controller, templateName) {
		this._super(controller, templateName);
	},

    delete: function () {
        this.root.remove();
    }
})