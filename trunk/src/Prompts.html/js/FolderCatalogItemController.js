var FolderCatalogItemController = function(model){
    this.model = model;
    this.toggleState = 'open';

    this.changeToggle = function () {
        if (this.toggleState === 'closed') {
            this.toggleState = 'open';
        } else if (this.toggleState === 'open') {
            this.toggleState = 'closed';
        }

        if (this.toggleState === 'closed') {
            this.view.renderCollapse();
        } else if (this.toggleState === 'open') {
            this.view.renderExpand();
        }
    };

    this.Select = function () {
    };

    this.UnSelect = function () {
    };

    this.CreateView = function(){
        this.setView( new FolderCatalogItemView(this) );
        return this.view;
    };

    this.setView = function (val) {
        this.view = val;
    }
};