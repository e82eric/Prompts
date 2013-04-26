var PromptsView = Class.extend({
    init: function(controller) {
        this.controller = controller;
        this.executeReportButton = $("#execute-report button");
        this.root = $("<ul></ul>");
    },

    render: function () {
        this.executeReportButton.click($.proxy(this.controller.onExecuteClick, this.controller));
        return this.root;
    },

    renderItems: function(items) {
        _.each(
            items,
            function (item) {
                var itemView = item.createView();
                this.root.append(itemView.render());
            },
            this);
    },

    setExecutionIndicatorReady: function () {
        this.executeReportButton.removeAttr('disabled');
    },

    setExecutionIndicatorNotReady: function () {
        this.executeReportButton.attr('disabled','disabled');
    }
});