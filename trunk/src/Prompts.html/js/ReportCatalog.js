$(function () {
    var reportCatalogBuilder = new ReportCatalogBuilder();
	var catalogRepository = new CatalogRepository(reportCatalogBuilder);
	var loadingPanel = new ReportCatalogPanel({reportCatalogRepository: catalogRepository});

    new ReportCatalogPanelView({model:loadingPanel});
});

function ReportCatalog (hierarchyFlattener, singleSelector) {
    this.hierarchyFlattener = hierarchyFlattener;
    this.singleSelector = singleSelector;

    this.setItems = function (val) {
        this.items = val;
    }

    this.Select = function (item) {
        var flattenedItems = this.hierarchyFlattener.Flatten(this.items);
        this.singleSelector.Select(flattenedItems, item);
    }
}

