var EmptyPromptController = PromptController.extend({
    init: function (model, promptsController, createViewFunc) {
    	this._super(model, promptsController, createViewFunc);
    },

    onTextChanged: function (val) {
    	this.text = val;
    	this.setReadyForExecution();
    },

    selections: function () {
        return [];
    },

    evaluateReadyForExecution: function () {
        return this.text == null || this.text === '';
    }
});