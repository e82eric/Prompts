var PromptView = TemplateView.extend({
	init: function (controller, templateName) {
		this._super(controller, templateName);
        this.readyIndicator = this.root.find(".ready-indicator");
	},

    deleteItem: function () {
        this.root.remove();
    },

    setExecutionIndicatorReady: function () {
        this.readyIndicator.text("ready");
    },

    setExecutionIndicatorNotReady: function () {
        this.readyIndicator.text("not ready");
    }
})