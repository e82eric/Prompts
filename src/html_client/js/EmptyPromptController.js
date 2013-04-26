var EmptyPromptController = PromptController.extend({
    init: function (model, createViewFunc) {
    	this._super(model, createViewFunc);
    },

    onTextChanged: function (val) {
    	this.text = val;
    	this.evaluateReadyForExecution();
    },

    evaluateReadyForExecution: function () {
    	if(this.text == null || this.text === '') {
    		this.view.setExecutionIndicatorNotReady();
    	} else {
    		this.view.setExecutionIndicatorReady();
    	}
    }
});