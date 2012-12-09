var PromptsRepository = function () {
    this.Get = function (path, successCallback, errorCallback) {
        $.ajax({
            type:"POST",
            contentType:"application/json; charset=utf-8",
            dataType:"json",
            data: {Path: path},
            url:"/prompts.service/api/prompts",
            context: this,
            success:function (result) {
            },
            error:function (xhr, statusText, errorThrown) {
            }
        });
    }
};