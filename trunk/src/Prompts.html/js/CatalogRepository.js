function CatalogRepository(reportCatalogBuilder) {
  this.reportCatalogBuilder = reportCatalogBuilder;

  this.GetCatalog = function (successCallback){
      var localReportCatalogBuilder;
      localReportCatalogBuilder = this.reportCatalogBuilder;

    $.ajax({
      type: "GET",
      contentType: "application/json; charset=utf-8",
      dataType: "json",
      url: "/prompts.service/api/reports",
      success: function (result) {
        var catalog = localReportCatalogBuilder.Build(result);
        successCallback(catalog);
      },
        error:function () {
            alert("AJAX Error!");
      }
    });
  }
}