var EmptyPromptController = Class.extend({
    init: function (model) {
        this.model = model;
    },

    createView: function () {
        return new EmptyPromptView(this);
    }
});