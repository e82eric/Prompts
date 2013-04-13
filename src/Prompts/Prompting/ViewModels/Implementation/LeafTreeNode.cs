using System.Collections.ObjectModel;
using Prompts.Service.ReportExecution;

namespace Prompts.Prompting.ViewModels.Implementation
{
    public class LeafTreeNode : TreeNode
    {
        public LeafTreeNode(
            string promptName
            , string parameterName
            , ValidValue validValue
            , ITreeNode parent)
            : base(
                promptName
                , parameterName
                , validValue
                , new ObservableCollection<ITreeNode>()
                , parent
                , true
                , false)
        {
        }

        public override bool IsExpanded
        {
            get { return false; }
            set { }
        }
    }
}