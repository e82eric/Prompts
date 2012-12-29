var AsynchronousSearchShoppingCartView = Class.extend({
    init: function (controller, loadingPanelView) {
        this.controller = controller;

        this.root = $("<li></li>");
        var template = $("#asynchronousSearchShoppingCartTemplate").html();

        var templateFunction  = _.template(template);
        var templateHtml = templateFunction(this.controller.model);
        this.root.html(templateHtml);

        this.selectButton = loadingPanelView.root.find("#selectButton");
        this.unSelectButton = loadingPanelView.root.find("#unSelectButton");
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
    }
});