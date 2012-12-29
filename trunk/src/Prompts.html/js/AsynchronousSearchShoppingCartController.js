var AsynchronousSearchShoppingCartController = Class.extend({
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

    createView: function () {
        var loadingPanelView = this.loadingPanel.createView(
            this.availableItemsController.createView(),
            this.selectedItemsController.createView()
        );

        this.view = new AsynchronousSearchShoppingCartView(this, loadingPanelView);
        return this.view;
    }
});