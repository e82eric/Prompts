var Repository = Class.extend({
    init: function (builder, url) {
        this.builder = builder;
        this.url = url;
    },

    Get: function (request, successCallback, errorCallback) {
        $.ajax({
            type:"POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(request),
            url:this.url,
            context: this,
            success:function (result) {
                var controller = this.builder.build(result);
                successCallback(controller);
            },
            error:function (xhr, statusText, errorThrown) {
                errorCallback(JSON.parse(xhr.responseText).ResponseStatus.Message);
            }
        });
    }
});