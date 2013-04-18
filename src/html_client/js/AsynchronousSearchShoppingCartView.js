var AsynchronousSearchShoppingCartView = PromptView.extend({
    init: function (controller, loadingPanelView) {
        this._super(controller, "asynchronousSearchShoppingCartTemplate");
        this.loadingPanelView = loadingPanelView;
        this.selectButton = this.loadingPanelView.root.find("#selectButton");
        this.unSelectButton = this.loadingPanelView.root.find("#unSelectButton");
        this.searchStringInput = this.root.find("#searchString");
        this.searchButton = this.root.find("#searchButton");
        this.content = this.root.find("#content");

        this.content.append(loadingPanelView.render());
    },

    render: function () {
        this.selectButton.click($.proxy(this.onSelectButtonClick, this));
        this.unSelectButton.click($.proxy(this.onUnSelectButtonClick, this));
        this.searchButton.click($.proxy(this.onSearch, this));
        this.searchStringInput.change($.proxy(this.onSearchStringChange, this));
        this.loadingPanelView.root.filter("#retry").click($.proxy(this.onRetryClick, this));

        return this.root;
    },

    onSelectButtonClick: function () {
        this.controller.onSelect();
    },

    onUnSelectButtonClick: function () {
        this.controller.onUnSelect();
    },

    onSearch: function () {
        this.controller.onSearchButtonClicked();
    },

    onSearchStringChange: function () {
        this.controller.onSearchStringSet(this.searchStringInput.val());
    },

    onRetryClick: function () {
        this.controller.onRetryClick();
    }
});