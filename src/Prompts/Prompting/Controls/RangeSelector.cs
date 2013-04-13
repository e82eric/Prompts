using System.Collections.Generic;
using System.Linq;

namespace Prompts.Prompting.Controls
{
    public class RangeSelector : IRangeSelector
    {
        private readonly ITreeItemHierarchyFlattener _hierarchyFlattener;

        public RangeSelector(ITreeItemHierarchyFlattener hierarchyFlattener)
        {
            _hierarchyFlattener = hierarchyFlattener;
        }

        public void Select(ICollection<ITreeItem> rootItems, ITreeItem item1, ITreeItem item2)
        {
            var flatItemsList = _hierarchyFlattener.Flatten(rootItems).ToList();
            var indexOfItem1 = flatItemsList.IndexOf(item1);
            var indexOfItem2 = flatItemsList.IndexOf(item2);

            ITreeItem firstItem;
            ITreeItem secondItem;

            if(indexOfItem1 < indexOfItem2)
            {
                firstItem = item1;
                secondItem = item2;
            }
            else
            {
                firstItem = item2;
                secondItem = item1;
            }

            var firstItemFound = false;
            var secondItemFound = false;
            foreach (var treeItem in flatItemsList)
            {
                if (treeItem == firstItem)
                {
                    firstItemFound = true;
                }
                if (firstItemFound && !secondItemFound)
                {
                    treeItem.IsSelected2 = true;
                }
                else
                {
                    treeItem.IsSelected2 = false;
                }
                if (treeItem == secondItem)
                {
                    secondItemFound = true;
                }
            }
        }
    }
}
