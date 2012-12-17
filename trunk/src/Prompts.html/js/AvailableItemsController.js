function AvailableItemsController (selector) {
    this.selector = selector;

    this.createView = function () {
        return new AvailableItemsView(this);
    }

    this.select = function (item) {
        this.selector.Select(this.items, item);
    }

    this.setItems = function (val) {
        this.items = val;
    }
}