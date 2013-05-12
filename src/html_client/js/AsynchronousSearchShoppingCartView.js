var AsynchronousSearchShoppingCartView = PromptView.extend({
    init: function (controller, loadingPanelView) {
        this._super(controller, "asynchronousSearchShoppingCartTemplate");
        this.loadingPanelView = loadingPanelView;
        this.selectButton = this.loadingPanelView.root.find(".select-button");
        this.unSelectButton = this.loadingPanelView.root.find(".unselect-button");
        this.searchStringInput = this.root.find(".search-input");
        this.searchButton = this.root.find(".search-button");
        this.content = this.root.find(".content");

        this.content.append(loadingPanelView.render());
    },

    render: function () {
        this.selectButton.click($.proxy(this.onSelectButtonClick, this));
        this.unSelectButton.click($.proxy(this.onUnSelectButtonClick, this));
        this.searchButton.click($.proxy(this.onSearch, this));
        this.searchStringInput.change($.proxy(this.onSearchStringChange, this));
        this.loadingPanelView.root.filter(".retry").click($.proxy(this.onRetryClick, this));

        return this.root;
    },

    onSelectButtonClick: function () {
        this.controller.onSelect();
    },

    onUnSelectButtonClick: function () {
        this.controller.onUnSelect();
    },

    onSearch: function () {
        this.controller.availableItemsController.search();
    },

    onSearchStringChange: function () {
        this.controller.availableItemsController.setSearchString(this.searchStringInput.val());
    },

    onRetryClick: function () {
        this.controller.onRetryClick();
    }
});