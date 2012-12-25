function ReportCatalog (hierarchyFlattener, singleSelector) {
    this.hierarchyFlattener = hierarchyFlattener;
    this.singleSelector = singleSelector;

    this.setItems = function (val) {
        this.items = val;
    }

    this.Select = function (item) {
        var flattenedItems = this.hierarchyFlattener.Flatten(this.items);
        this.singleSelector.select(flattenedItems, item);
    }
}

