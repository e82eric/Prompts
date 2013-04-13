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
    var model = { Path: "Path 1" };

    var promptsPresenter = {};
    promptsPresenter.load = sinon.spy();

    var view = {};
    view.onSelected = sinon.spy();
    var controller = new ReportCatalogItemController(model, promptsPresenter);
    controller.setView(view);

    ok( !(view.onSelected.called) );

    controller.Select();

    ok ( view.onSelected.calledOnce );
});

test( "It calls render not selected on the view when unselect is called", function() {
    var view = {};
    view.onUnSelected = sinon.spy();
    var controller = new ReportCatalogItemController();
    controller.setView(view);

    ok( !(view.onUnSelected.called) );

    controller.UnSelect();

    ok ( view.onUnSelected.calledOnce );
});

test( "It calls load on the prompts controller where select is called", function() {
    var model = { Path: "Path 1" };

    var view = {};
    view.onSelected = sinon.spy();

    var promptsPresenter = {};
    promptsPresenter.load = sinon.spy();

    var controller = new ReportCatalogItemController(model, promptsPresenter);
    controller.setView(view);

    ok( !(view.onSelected.called) );

    controller.Select();

    ok ( view.onSelected.calledOnce );
    ok ( promptsPresenter.load.getCall(0).args[0].Path ==  model.Path);
});