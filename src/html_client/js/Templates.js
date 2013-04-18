var Prompts = {};
Prompts.Templates = {};

Prompts.Templates["promptingLoadingPanelTemplate"] = _.template([
	'<div id="loaded"></div>',
	'<div id="loading">loading...</div>',
	'<div id="errorMessage"></div>',
	'<a id="retry">Retry</a>'
].join(''));

Prompts.Templates["childPromptItemsLoadingPanelTemplate"] = _.template([
	'<div>',
	  '<ul class="childItems" id="loading">',
	      '<li class="treeNodeLoading">loading...</li>',
	  '</ul>',
	  '<div id="loaded">',
	  '</div>',
	  '<ul id="errorMessage">',
	  '</ul>',
	  '<ul id="retry">',
	      '<a>Retry</a>',
	  '</ul>',
	'</div>'
].join(''));

Prompts.Templates["itemTemplate"] = _.template([
	'<div id="wrap">',
	    '<div id="ExpandImage" class="catalogItem"/>',
	    '<div id="hoverWrap" class="catalogItem">',
	    '<div id="selectWrap" class="catalogItem">',
	        '<img src="../images/report.png" class="catalogItem" />',
	        '<div class="catalogItem"><%- model.Name %></div>',
	      '</div>',
	    '</div>',
	  '</div>'
].join(''));

Prompts.Templates["leafTreePromptItemTemplate"] = _.template([
  '<li class="treeItem" unselectable="on">',
  	'<span id="selectWrap">',
      	'<span class="itemTest"><%- Label %></span>',
  	'</span>',
  '</li>'
].join(''));

Prompts.Templates["treePromptItemTemplate"] = _.template([
  '<li class="treeItem" unselectable="on">',
  	'<div class="catalogItem" style="width:16px">',
          '<img id="expandImage" src="../images/tree_collapsed.png">',
      '</div>',
      '<span id="selectWrap">',
          '<span class="itemTest"><%- model.Label %></span>',
      '</span>',
    '</li>'
].join(''));

Prompts.Templates["folderItemTemplate"] = _.template([
  '<li class="ReportView">',
  	'<div id="wrap">',
  		'<div id="ExpandImage" class="catalogItem">',
  			'<img />',
  		'</div>',
  		'<img src="../images/folder.png" class="catalogItem" />',
  		'<div class="catalogItem"><%- model.Name %></div>',
  	'</div>',
  '</li>'
].join(''));

Prompts.Templates["leafTreePromptItemTemplate"] = _.template([
	'<span id="selectWrap">',
    	'<span class="itemTest"><%- model.Label %></span>',
	'</span>'
].join(''));

Prompts.Templates["emptyFolderItemTemplate"] = _.template([
	'<div id="wrap">',
	    '<div id="ExpandImage" class="catalogItem"></div>',
	    '<img src="../images/folder.png" class="catalogItem" />',
	    '<div class="catalogItem"><%- model.Name %></div>',
	'</div>'
].join(''));

Prompts.Templates["emptyPromptTemplate"] = _.template([
	'<input style="width:100%; height: 25px;">'
].join(''));

Prompts.Templates["treeDropDownTemplate"] = _.template([
  '<li>',
    '<div class="dropDown" >',
        '<div class="dropDownToggle" id="toggle" >',
            '<span id="selectedItemText"></span>',
            '<span><img class="dropDownToggleImage" src="../images/tree_expand.png"></span>',
        '</div>',
          '<div id="popup" class="dropDownPopup" >',
        '</div>',
    '</div>',
  '</li>'
].join(''));

Prompts.Templates["dropDownTemplate"] = _.template([
  '<li>',
  	'<div class="dropDown" >',
  	    '<div class="dropDownToggle" id="toggle" >',
  	        '<span id="selectedItemText"></span>',
  	        '<span><img class="dropDownToggleImage" src="../images/tree_expand.png"></span>',
  	    '</div>',
  	    '<div id="popup" class="dropDownPopup" >',
  	        '<button id="searchButton" style="float:right">Search</button>',
  	        '<div style="padding: 0 5px 0 2px; overflow: hidden;" >',
  	            '<input id="searchString" type="text" title="Search" style="width: 100%" />',
  	        '</div>',
  	    '</div>',
  	'</div>',
  '</li>'
].join(''));

Prompts.Templates["shoppingCartTemplate"] = _.template([
  '<li>',
    '<table style="height:400px; width: 100%;">',
        '<tr>',
            '<td><input id="searchString" style="width:100%"></td>',
            '<td><button id="searchButton" style="width:100%">Search</button></td>',
        '</tr>',
        '<tr>',
            '<td rowspan="2" class="listBox" style="width:50%">',
                '<div id="availableItems" style="height: 400px; overflow-y: scroll">',
                '</div>',
            '</td>',
            '<td style="width: 100px; vertical-align: bottom; height: 200px">',
               ' <button id="selectButton" style="width: 100px;">Select</button>',
            '</td>',
            '<td rowspan="2" class="listBox">',
                '<div style="height: 400px; overflow-y: scroll">',
                    '<ul id="selectedItems" class="rootItems">',
                    '</ul>',
                '</div>',
            '</td>',
        '</tr>',
        '<tr>',
            '<td style="vertical-align: top">',
                '<button id="unSelectButton" style="width: 100px;">De-Select</button>',
            '</td>',
        '</tr>',
    '</table>',
  '<li>'
].join(''));

Prompts.Templates["asynchronousSearchShoppingCartTemplate"] = _.template([
  '<li>',
  	'<table id="content" style="height:400px; width: 100%;">',
  	  '<tr style="vertical-align: top; height: 25px">',
  	      '<td><input id="searchString" style="width:100%"></td>',
  	      '<td><button id="searchButton" style="width:100%">Search</button></td>',
  	  '</tr>',
  	'</table>',
  '</li>'
].join(''));

Prompts.Templates["asynchronousSearchLoadingPanelTemplate"] = _.template([
  '<tr style="vertical-align: top" id="loading"><td >loading...</td></tr>',
  '<tr id="errorMessage">',
  '</tr>',
  '<tr style="vertical-align: top" id="retry">',
    '<td><a>Retry</a></td>',
  '</tr>',
  '<tr id="loaded">',
      '<td rowspan="2" class="listBox" style="width:50%">',
          '<div id="availableItems" style="height: 400px; overflow-y: scroll">',
          '</div>',
      '</td>',
      '<td style="width: 100px; vertical-align: bottom; height: 200px">',
          '<button id="selectButton" style="width: 100px;">Select</button>',
      '</td>',
      '<td rowspan="2" class="listBox">',
          '<div style="height: 400px; overflow-y: scroll">',
              '<ul id="selectedItems" class="rootItems">',
              '</ul>',
          '</div>',
      '</td>',
  '</tr>',
  '<tr id="loaded">',
      '<td style="vertical-align: top">',
          '<button id="unSelectButton" style="width: 100px;">De-Select</button>',
      '</td>',
  '</tr>'
].join(''));

Prompts.Templates["treeShoppingCartTemplate"] = _.template([
  '<li>',
    '<table style="height:400px; width: 100%;">',
        '<tr>',
            '<td rowspan="2" class="listBox" style="width:50%">',
                '<div id="availableItems" style="height: 400px; overflow-y: scroll">',
                '</div>',
            '</td>',
            '<td style="width: 100px; vertical-align: bottom; height: 200px">',
                '<button id="selectButton" style="width: 100px;">Select</button>',
            '</td>',
            '<td rowspan="2" class="listBox">',
                '<div style="height: 400px; overflow-y: scroll">',
                    '<ul id="selectedItems" class="rootItems">',
                    '</ul>',
                '</div>',
            '</td>',
        '</tr>',
        '<tr>',
            '<td style="vertical-align: top">',
                '<button id="unSelectButton" style="width: 100px;">De-Select</button>',
            '</td>',
        '</tr>',
    '</table>',
  '</li>'
].join(''));

Prompts.Templates["availableItemsTemplate"] = _.template([
	'<ul id="availableItems" class="rootItems">',
	'</ul>'
].join(''));



