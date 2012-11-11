using System.Collections.Generic;

namespace Prompts.Prompting.Controls
{
    public interface ITreeItemHierarchyFlattener
    {
        ICollection<ITreeItem> Flatten(ICollection<ITreeItem> rootTreeItems);
    }
}