var PromptController = Class.extend({
	init: function (model, promptsController, createViewFunc) {
		this.model = model;
		this.promptsController = promptsController;
		this.createViewFunc = createViewFunc;
	},

	deleteItem: function () {
		this.view.deleteItem();
	},

	onSmallHeaderClick: function () {
		this.promptsController.moveToPrompt(this);		
	},

	setReadyForExecution: function () {
        if(this.evaluateReadyForExecution()) {
        	this.readyForExecution = true;
            this.view.setExecutionIndicatorReady();
        } else {
            this.view.setExecutionIndicatorNotReady();
            this.readyForExecution = false;
        }
        this.promptsController.evaluateReadyForExecution();
    },

    selectionInfo: function () {
        return { PromptName: this.model.Name, Selections: this.selections() };
    },

	setView: function(val) {
		this.view = val;
		this.setReadyForExecution();
        return this.view;
	},

    createView: function () {
        return this.setView(this.createViewFunc(this));
    },

	showSmall: function () {
		this.view.showSmall();
	},

	hideSmall: function () {
		this.view.hideSmall();
	}
});
