module( "Report Catalog Item Controller Tests");

test( "It calls select on the report catalog when change select is called", function() {
    var catalog = {};
    catalog.Select = sinon.spy();
    var controller = new ReportCatalogItemController();
    controller.setRepository(catalog);

    ok( !(catalog.Select.called) );

    controller.changeSelect();

    ok ( catalog.Select.withArgs(controller).calledOnce );
});

test( "It calls render selected on the view when select is called", function() {
    var view = {};
    view.onSelected = sinon.spy();
    var controller = new ReportCatalogItemController();
    controller.setView(view);

    ok( !(view.onSelected.called) );

    controller.Select();

    ok ( view.onSelected.calledOnce );
});

test( "It calls render not selected on the view when select is called", function() {
    var view = {};
    view.onUnSelected = sinon.spy();
    var controller = new ReportCatalogItemController();
    controller.setView(view);

    ok( !(view.onUnSelected.called) );

    controller.UnSelect();

    ok ( view.onUnSelected.calledOnce );
});