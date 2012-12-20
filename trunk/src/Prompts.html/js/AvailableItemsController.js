function AvailableItemsController (selector) {
    this.selector = selector;

    this.createView = function () {
        return new AvailableItemsView(this);
    }

    this.select = function (shiftKeyPressed, controlKeyPressed, item) {
        this.selector.select(shiftKeyPressed, controlKeyPressed, this.items, item);
    }

    this.setItems = function (val) {
        this.items = val;
    }
}