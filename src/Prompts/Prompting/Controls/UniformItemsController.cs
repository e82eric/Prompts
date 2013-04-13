using System.Collections.Generic;
using System.Linq;

namespace Prompts.Prompting.Controls
{
    public class UniformItemsController
    {
        private readonly IUniformItemsControl _uniformItemsControl;
        private object _selectedItem;
        private IEnumerable<object> _items;
        private bool _invalid;
        private readonly Queue<SwitchHelper> _itemQue;

        public UniformItemsController(IUniformItemsControl uniformItemsControl)
        {
            _uniformItemsControl = uniformItemsControl;
            _itemQue = new Queue<SwitchHelper>();
        }

        private void CreateItems()
        {
            _uniformItemsControl.CreateItems(Items);
            _uniformItemsControl.HideItem(_selectedItem);
            _invalid = false;
        }

        private bool EvaluateIfControlHasBeenInitialized()
        {
            return SelectedItem != null && Items != null;
        }

        public object SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                var originalSelectedItem = _selectedItem;
                _selectedItem = value;

                if(EvaluateIfControlHasBeenInitialized())
                {
                    if ((_invalid && EvaluateIfSelectedItemExistInItems()) || originalSelectedItem == null)
                    {
                        CreateItems();
                    }
                    
                    else if (originalSelectedItem != _selectedItem)
                    {
                        SwitchIfSelectedItemExistsInItemsElseInvalidate(originalSelectedItem);
                    }
                }
            }
        }

        private void SwitchIfSelectedItemExistsInItemsElseInvalidate(object originalSelectedItem)
        {
            InvalidateIfSelectedItemDoesNotExistInItems();
            if(EvaluateIfSelectedItemExistInItems())
            {
                var switchHelper = new SwitchHelper(originalSelectedItem, _selectedItem);

                _itemQue.Enqueue(switchHelper);
                if(_itemQue.Count == 1)
                {
                    SwitchItem(switchHelper);
                }
            }
        }

        private void InvalidateIfSelectedItemDoesNotExistInItems()
        {
            if (EvaluateIfSelectedItemExistInItems() == false)
            {
                _invalid = true;
            }
        }

        private void SwitchItem(SwitchHelper switchHelper)
        {
            _uniformItemsControl.HideItem(switchHelper.ItemToShrink);
            _uniformItemsControl.GrowItem(switchHelper.ItemToGrow, OnGrowItemCompleted);
        }

        private void OnGrowItemCompleted()
        {
            _itemQue.Dequeue();
            if (_itemQue.Count > 0)
            {
                var itemSwitch = _itemQue.Peek();
                SwitchItem(itemSwitch);
            }
        }

        private bool EvaluateIfSelectedItemExistInItems()
        {
            return Items.Where(i => i == _selectedItem).SingleOrDefault() != null;
        }

        public IEnumerable<object> Items
        {
            private get { return _items; }
            set
            {
                var originalItems = _items;
                _items = value;

                if(EvaluateIfControlHasBeenInitialized())
                {
                    if (originalItems == null || _invalid)
                    {
                        CreateItems();
                    }

                    InvalidateIfSelectedItemDoesNotExistInItems();
                }  
            }
        }

        private class SwitchHelper
        {
            private readonly object _itemToGrow;
            private readonly object _itemToShrink;

            public SwitchHelper(object itemToGrow, object  itemToShrink)
            {
                _itemToGrow = itemToGrow;
                _itemToShrink = itemToShrink;
            }

            public object ItemToShrink
            {
                get { return _itemToShrink; }
            }

            public object ItemToGrow
            {
                get { return _itemToGrow; }
            }
        }
    }
}
