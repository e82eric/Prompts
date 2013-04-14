var EmptyFolderCatalogItemController = SelectableItemController.extend({
    init: function(model){
        this.model = model;
        this.Children = [];
    },

    createView: function(){
        return new ReportCatalogItemViewBase("#emptyFolderItemTemplate", this);
    },

    select: function () {},

    unSelect: function () {}
});
