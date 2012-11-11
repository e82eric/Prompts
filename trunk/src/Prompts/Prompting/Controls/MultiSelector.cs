using System.Collections.Generic;
using System.Windows.Input;

namespace Prompts.Prompting.Controls
{
    public class MultiSelector
    {
        private readonly IRangeSelector _rangeSelector;
        private bool _previousSelectionWasShift;
        private ITreeItem _previousShiftSelection;
        private readonly ISelector _inverseSelector;
        private readonly ISelector _nullSelector;

        public MultiSelector(
            IRangeSelector rangeSelector,
            ISelector inverseSelector,
            ISelector nullSelector)
        {
            _nullSelector = nullSelector;
            _inverseSelector = inverseSelector;
            _rangeSelector = rangeSelector;
        }

        public void Select(ModifierKeys modifierKey, ICollection<ITreeItem> rootTreeItems, ITreeItem newSelectedItem, ITreeItem oldSelectedItem)
        {
            switch (modifierKey)
            {
                case ModifierKeys.Shift:
                    if(_previousSelectionWasShift)
                    {
                        _rangeSelector.Select(rootTreeItems, newSelectedItem, _previousShiftSelection);
                    }
                    else
                    {
                        _rangeSelector.Select(rootTreeItems, newSelectedItem, oldSelectedItem);
                        _previousShiftSelection = oldSelectedItem;
                    }
                    _previousSelectionWasShift = true;
                    break;
                case ModifierKeys.Control:
                    _previousSelectionWasShift = false;
                    _inverseSelector.Select(rootTreeItems, newSelectedItem);
                    break;
                default:
                    _previousSelectionWasShift = false;
                    _nullSelector.Select(rootTreeItems, newSelectedItem);
                    break;
            }
        }
    }
}
