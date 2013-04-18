var EmptyPromptController = PromptController.extend({
    init: function (model) {
        this.model = model;
    },

    createView: function () {
    	this.view = new EmptyPromptView(this);
        return this.view;
    }
});