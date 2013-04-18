var EmptyPromptView = PromptView.extend({
    init: function (controller) {
        this._super(controller, "emptyPromptTemplate");
    },
    render: function () {
        return this.root;
    }
});