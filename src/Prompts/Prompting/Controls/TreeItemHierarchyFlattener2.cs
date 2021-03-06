﻿using System.Collections.Generic;

namespace Prompts.Prompting.Controls
{
    public class TreeItemHierarchyFlattener2 : ITreeItemHierarchyFlattener
    {
        private ICollection<ITreeItem> _rootTreeItems;
        private readonly List<ITreeItem> _denormalizedItems;

        public TreeItemHierarchyFlattener2()
        {
            _denormalizedItems = new List<ITreeItem>();
        }

        private void AddChildren(ITreeItem rootTreeItem)
        {
            if (rootTreeItem.Children != null)
            {
                foreach (var child in rootTreeItem.Children)
                {
                    _denormalizedItems.Add(child);
                    AddChildren(child);
                }
            }
        }

        public ICollection<ITreeItem> Flatten(ICollection<ITreeItem> rootTreeItems)
        {
            _rootTreeItems = rootTreeItems;
            _denormalizedItems.Clear();
            foreach (var rootTreeItem in _rootTreeItems)
            {
                _denormalizedItems.Add(rootTreeItem);
                AddChildren(rootTreeItem);
            }
            return _denormalizedItems;
        }
    }
}