var PromptController = Class.extend({
	init: function (model, promptsController, createViewFunc) {
		this.model = model;
		this.promptsController = promptsController;
		this.createViewFunc = createViewFunc;
	},

	delete: function () {
		this.view.delete();
	},

	setReadyForExecution: function () {
        if(this.evaluateReadyForExecution()) {
        	this.readyForExecution = true;
            this.view.setExecutionIndicatorReady();
            this.promptsController.evaluateReadyForExecution();
        } else {
            this.view.setExecutionIndicatorNotReady();
            this.readyForExecution = false;
            this.promptsController.evaluateReadyForExecution();
        }
    },

    selectionInfo: function () {
        return { Name: this.model.Name, Selections: this.selections() };
    },

	setView: function(val) {
		this.view = val;
		this.setReadyForExecution();
	},

    createView: function () {
        this.setView(this.createViewFunc());
        return this.view;
    }
});