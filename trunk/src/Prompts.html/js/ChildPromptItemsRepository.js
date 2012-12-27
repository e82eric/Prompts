function ChildPromptItemsRepository (builder) {
    this.builder = builder;
    this.Get = function (request, successCallback, errorCallback) {
        $.ajax({
            type:"POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(request),
            url:"/Prompts.Service/api/prompts/child_items",
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
};