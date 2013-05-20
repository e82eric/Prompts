var PromptsView = TemplateView.extend({
    init: function(controller) {
        this._super(controller, "promptsTemplate");
    	this.promptElement = this.root.filter("#prompts-list");
		this.nextButton = this.root.find("#next-button");
		this.previousButton = this.root.find("#previous-button");
	},

    render: function () {
		this.nextButton.click($.proxy(this.controller.moveNext, this.controller));
		this.previousButton.click($.proxy(this.controller.movePrevious, this.controller));
        return this.root;
    },

	displayPrompt: function (val) {
		this.promptElement.html(val.createView().render());
	},

	enableMoveNext: function () {
		this.nextButton.removeAttr("disabled");
	},

	disableMoveNext: function () {
		this.nextButton.attr("disabled", "disabled");
	},

	enableMovePrevious: function () {
		this.previousButton.removeAttr("disabled", "disabled");
	},

	disableMovePrevious: function () {
		this.previousButton.attr("disabled", "disabled");
	}
});
