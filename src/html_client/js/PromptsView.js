var PromptsView = TemplateView.extend({
    init: function(controller) {
        this._super(controller, "promptsTemplate");
    	this.promptElement = this.root.filter("#prompts-list");
		this.smallPromptsElement = this.root.find("#small-prompts-list");
		this.nextButton = this.root.find("#next-button");
		this.previousButton = this.root.find("#previous-button");
	},

    render: function () {
		this.nextButton.click($.proxy(this.controller.moveNext, this.controller));
		this.previousButton.click($.proxy(this.controller.movePrevious, this.controller));
        return this.root;
    },

	displayPrompt: function (val) {
		if(this.displayedPrompt != undefined) {
			this.displayedPrompt.view.root.remove();
		}

		this.displayedPrompt = val;
		var itemView = undefined;
		if(val.view == undefined) {
			itemView = val.createView();
		} else {
			itemView = val.view;
		}			
		this.promptElement.append(itemView.render());
		this.promptElement.hide();
		this.promptElement.fadeIn();
	},

	setSmallItems: function (val) {
		this.smallPromptsElement.empty();
		var rowHeight = ((1 / (val.length - 1)) * 100) + "%";
		_.each(
            val,
            function (controller) {
				var itemView= undefined;
				if(controller.view == undefined) {
					itemView = controller.createView();
				} else {
                	itemView = controller.view;
                }
				var rItem = itemView.renderSmall();
				rItem.height(rowHeight);
				this.smallPromptsElement.append(rItem);
            },
            this
        );

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
