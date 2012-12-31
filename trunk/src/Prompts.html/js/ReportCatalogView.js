var ReportCatalogView = Class.extend({
   init: function(controller, loadingPanelView) {
       this.root = $("#reportCatalog");
       this.root.append(loadingPanelView.render());
   },

   render: function () {
       return this.root;
   }
});