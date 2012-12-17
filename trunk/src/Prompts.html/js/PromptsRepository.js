var PromptsRepository = function (builder) {
    this.builder = builder;
    this.Get = function (request, successCallback, errorCallback) {
        $.ajax({
            type:"POST",
            dataType:"json",
            data: request,
            url:"/Prompts.Service/api/Prompts",
            context: this,
            success:function (result) {
                var view = this.builder.build(result);
                successCallback(view);
            },
            error:function (xhr, statusText, errorThrown) {
                errorCallback(JSON.parse(xhr.responseText).ResponseStatus.Message);
            }
        });
    }
};