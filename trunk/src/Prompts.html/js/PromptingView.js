var PromptingView = Class.extend({
    init: function (controller, loadingPanelView) {
        this.root = $("#prompts");
        this.root.append(loadingPanelView.render());
    },

    render: function () {
        return this.root;
    }
});