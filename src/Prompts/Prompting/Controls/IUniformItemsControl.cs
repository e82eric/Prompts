using System;
using System.Collections.Generic;

namespace Prompts.Prompting.Controls
{
    public interface IUniformItemsControl
    {
        void CreateItems(IEnumerable<object> items);
        void GrowItem(object item, Action completed);
        void HideItem(object item);
    }
}
