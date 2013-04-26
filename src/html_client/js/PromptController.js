var PromptController = Class.extend({
	init: function (model, createViewFunc) {
		this.model = model;
		this.createViewFunc = createViewFunc;
	},

	delete: function () {
		this.view.delete();
	},

	setView: function(val) {
		this.view = val;
		this.evaluateReadyForExecution();
	},

    createView: function () {
        this.setView(this.createViewFunc());
        return this.view;
    }
});