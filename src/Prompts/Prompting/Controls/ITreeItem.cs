using System.Collections.Generic;

namespace Prompts.Prompting.Controls
{
    public interface ITreeItem
    {
        bool IsSelected2 { get; set; }
        ICollection<ITreeItem> Children { get; }
        bool IsExpanded { get; set; }
    }
}
