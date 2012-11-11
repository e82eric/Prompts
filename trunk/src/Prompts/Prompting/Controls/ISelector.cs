using System.Collections.Generic;

namespace Prompts.Prompting.Controls
{
    public interface ISelector
    {
        void Select(ICollection<ITreeItem> items, ITreeItem item);
    }
}