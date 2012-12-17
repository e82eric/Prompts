function DropDownView (controller){
    this.controller = controller;

    this.render = function (){
        this.root = $("<li></li>");

        var template = $("#dropDownTemplate").html();

        var templateFunction  = _.template(template);
        var templateHtml = templateFunction(this.controller.model);
        this.root.html(templateHtml);

        return this.root;
    }
}