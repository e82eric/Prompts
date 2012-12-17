var CatalogRepository = function (reportCatalogBuilder) {
    this.reportCatalogBuilder = reportCatalogBuilder;

    this.Get = function (request, successCallback, errorCallback) {
        $.ajax({
            type:"GET",
            contentType:"application/json; charset=utf-8",
            dataType:"json",
            url:"/prompts.service/api/reports",
            context: this,
            success:function (result) {
                var catalog = this.reportCatalogBuilder.Build(result);
                successCallback(catalog);
            },
            error:function (xhr, statusText, errorThrown) {
                errorCallback(JSON.parse(xhr.responseText).ResponseStatus.Message);
            }
        });
    }
};