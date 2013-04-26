var AsynchronousSearchShoppingCartController = MultiSelectPromptController.extend({
    init: function (
        model, 
        asynchronousSearch, 
        availableItemsController, 
        selectedItemsController, 
        loadingPanel,
        promptsController) {
        this._super(model, availableItemsController, selectedItemsController, promptsController);
        this.asynchronousSearch = asynchronousSearch;
        this.loadingPanel = loadingPanel;
    },

    onSearchStringSet: function (value) {
        this.searchString = value;
    },

    onSearchButtonClicked: function () {
        this.asynchronousSearch.execute(this.searchString, this.availableItemsController);
    },

    onRetryClick: function () {
        this.asynchronousSearch.executeLastRequest(this.availableItemsController);
    },

    createView: function () {
        var loadingPanelView = this.loadingPanel.createView();

        this.setView(new AsynchronousSearchShoppingCartView(this, loadingPanelView));
        return this.view;
    }
});