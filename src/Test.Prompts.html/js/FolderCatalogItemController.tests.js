module( "Folder Catalog Item Controller Tests");

test( "It calls the collapse delegate when it is called the first time", function() {
    var view = {};
    view.renderCollapse = sinon.spy();

	var folder = new FolderCatalogItemController({Children: [{Name: "Child"}]});
    folder.setView(view);
    folder.changeToggle();

    ok(view.renderCollapse.calledOnce)
});

test( "It calls the expand delegate when change toggle is called", function() {  
    var view = {};
    view.renderCollapse = sinon.spy();
    view.renderExpand = sinon.spy();

	var folder = new FolderCatalogItemController({Children: [{Name: "Child"}]});
    folder.setView(view);
	folder.changeToggle();

    ok(view.renderCollapse.calledOnce);
    ok(view.renderExpand.called == false);

	folder.changeToggle();

    ok(view.renderCollapse.calledOnce);
    ok(view.renderExpand.calledOnce);
});

test( "It calls the collapse delegate when change toggle is called again", function() {
    var view = {};
    view.renderCollapse = sinon.spy();
    view.renderExpand = sinon.spy();

    var folder = new FolderCatalogItemController({Children: [{Name: "Child"}]});
    folder.setView(view);
    folder.changeToggle();

    ok(view.renderCollapse.calledOnce);
    ok(view.renderExpand.called == false);

    folder.changeToggle();

    ok(view.renderCollapse.calledOnce);
    ok(view.renderExpand.calledOnce);

    folder.changeToggle();

    ok(view.renderCollapse.calledTwice);
    ok(view.renderExpand.calledOnce);
});

test( "It calls the expand delegate when change toggle is called for a third time", function() {
    var view = {};
    view.renderCollapse = sinon.spy();
    view.renderExpand = sinon.spy();

    var folder = new FolderCatalogItemController({Children: [{Name: "Child"}]});
    folder.setView(view);
    folder.changeToggle();

    ok(view.renderCollapse.calledOnce);
    ok(view.renderExpand.called == false);

    folder.changeToggle();

    ok(view.renderCollapse.calledOnce);
    ok(view.renderExpand.calledOnce);

    folder.changeToggle();

    ok(view.renderCollapse.calledTwice);
    ok(view.renderExpand.calledOnce);

    folder.changeToggle();

    ok(view.renderCollapse.calledTwice);
    ok(view.renderExpand.calledTwice);
});