var EmptyPromptView = PromptView.extend({
    init: function (controller) {
        this._super(controller, "emptyPromptTemplate");
        this.readyIndicator = this.root.find(".ready-indicator");
        this.textInput = this.root.find(".search-input");
    },

    render: function () {
    	this.textInput.keyup($.proxy(this.onTextChanged, this));
        return this.root;
    },

    onTextChanged: function () {
    	this.controller.onTextChanged(this.textInput.val());
    }
});