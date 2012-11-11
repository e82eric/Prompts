using System.Collections.Generic;

namespace Prompts.Prompting.Controls
{
    public class InverseSelector : ISelector
    {
        private readonly ITreeItemHierarchyFlattener _hierarchyFlattener;

        public InverseSelector(ITreeItemHierarchyFlattener hierarchyFlattener)
        {
            _hierarchyFlattener = hierarchyFlattener;
        }

        public void Select(ICollection<ITreeItem> items, ITreeItem item)
        {
            var flattenedItems = _hierarchyFlattener.Flatten(items);

            foreach (var treeItem in flattenedItems)
            {
                if (item == treeItem)
                {
                    item.IsSelected2 = item.IsSelected2 ? false : true;
                }
            }
        }
    }
}
