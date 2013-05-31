var PromptsController = DisposableItemsController.extend({
	init:function (itemsDisposer) {
		this._super(itemsDisposer);
	},

	moveNext: function () {
		var index = _.indexOf(this.items, this.currentItem); 
		this._setDisplayItem(this.items[index + 1]);
		this._evaluateMoveNextAndPrevious();
	},

	movePrevious: function () {
		var index = _.indexOf(this.items, this.currentItem);
		this._setDisplayItem(this.items[index - 1]);
		this._evaluateMoveNextAndPrevious();
	},

	moveToPrompt: function (val) {
		this._setDisplayItem(val);
		this._evaluateMoveNextAndPrevious();
	},

	_evaluateCanMovePrevious: function () {
		if(_.indexOf(this.items, this.currentItem) === 0) {
			this._canMovePrevious = false;
			this.view.disableMovePrevious();
		} else {
			if(this._canMovePrevious === false) {
				this.view.enableMovePrevious();
			}
			this._canMovePrevious = true;
		}
	},

	_evaluateCanMoveNext: function () {
		if(_.indexOf(this.items, this.currentItem) === this.items.length -1) {
			this.view.disableMoveNext();
			this._canMoveNext = false;
		} else {
			if(this._canMoveNext !== true) {
				this.view.enableMoveNext();
			}
			this._canMoveNext = true;
		}
	},

	_evaluateMoveNextAndPrevious: function () {
		this._evaluateCanMoveNext();
		this._evaluateCanMovePrevious();
	},

	_setDisplayItem: function (val) {
		if(this.currentItem != undefined) {
			this.currentItem.showSmall();
		}
		val.hideSmall();
		this.currentItem = val;
		this.view.displayPrompt(val);
	},

	setItems: function (val) {
		this.items = val;
		this.view.setSmallItems(val);
		this._setDisplayItem(this.items[0]);
		this._evaluateMoveNextAndPrevious();
	}
});
