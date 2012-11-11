using System.Collections.Generic;

namespace Prompts.Prompting.Controls
{
    public class Selector : ISelector
    {
        private readonly ITreeItemHierarchyFlattener _hierarchyFlattener;

        public Selector(ITreeItemHierarchyFlattener hierarchyFlattener)
        {
            _hierarchyFlattener = hierarchyFlattener;
        }

        public void Select(ICollection<ITreeItem> items, ITreeItem item)
        {
            var flattenedItems = _hierarchyFlattener.Flatten(items);

            foreach (var treeItem in flattenedItems)
            {
                treeItem.IsSelected2 = treeItem == item;
            }
        }
    }
}
