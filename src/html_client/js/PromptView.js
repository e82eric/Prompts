var PromptView = TemplateView.extend({
	init: function (controller, templateName) {
		this._super(controller, templateName);
        var smallTemplate = _.template($("#smallPromptTemplate").text());
        var smallTemplateHtml = smallTemplate(this.controller);
        this.smallRoot = $(smallTemplateHtml);
		this.smallContent = this.smallRoot.find(".small-prompt");
		this.smallHeader = this.smallRoot.find(".small-prompt-header");
		this.header = this.root.filter(".prompt-header");
        this.readyIndicator = this.root.find(".ready-indicator");
	},

	render: function () {
		return this._super();
	},

    deleteItem: function () {
        this.root.remove();
    },

    setExecutionIndicatorReady: function () {
		this.header.attr("class", "small-prompt-header prompt-header execution-ready");
		this.smallHeader.attr("class", "small-prompt-header prompt-header execution-ready");
    },

    setExecutionIndicatorNotReady: function () {
		this.header.attr("class", "small-prompt-header prompt-header not-execution-ready");
		this.smallHeader.attr("class", "small-prompt-header prompt-header not-execution-ready");
    },

	renderSmall: function () {
		this.smallHeader.click($.proxy(this.controller.onSmallHeaderClick, this.controller));
		return this.smallRoot;	
	},

	hideSmall: function () {
		this.smallRoot.hide();
		this.smallContent.fadeOut('slow');
	},

	showSmall: function () {
		this.smallRoot.show();
		this.smallContent.fadeIn('slow');
	}
});
