using System.Collections.Generic;

namespace Prompts.Prompting.Controls
{
    public interface IRangeSelector
    {
        void Select(ICollection<ITreeItem> rootItems, ITreeItem item1, ITreeItem item2);
    }
}