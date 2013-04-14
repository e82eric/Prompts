var AsynchronousSearchShoppingCartController = PromptController.extend({
    init: function (model, asynchronousSearch, availableItemsController, selectedItemsController, loadingPanel) {
        this.model = model;
        this.asynchronousSearch = asynchronousSearch;
        this.availableItemsController = availableItemsController;
        this.selectedItemsController = selectedItemsController;
        this.loadingPanel = loadingPanel;
    },

    onSearchStringSet: function (value) {
        this.searchString = value;
    },

    onSearchButtonClicked: function () {
        this.asynchronousSearch.execute(this.searchString, this.availableItemsController);
    },

    onSelect: function () {
        var selectedAvailableItems = this.availableItemsController.getSelectedItems();
        this.selectedItemsController.addItems(selectedAvailableItems);
    },

    onUnSelect: function () {
        this.selectedItemsController.removeSelected();
    },

    onRetryClick: function () {
        this.asynchronousSearch.executeLastRequest(this.availableItemsController);
    },

    createView: function () {
        var loadingPanelView = this.loadingPanel.createView();

        this.view = new AsynchronousSearchShoppingCartView(this, loadingPanelView);
        return this.view;
    }
});