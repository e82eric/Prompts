using System;
using Prompts.Service.ReportExecution;

namespace Prompts.Prompting.ViewModels.Implementation
{
    public class ErrorTreeNode : TreeNode
    {
        public ErrorTreeNode(string label) 
            : base(null, null, new ValidValue {Label = label}, null, null, false, false)
        {
        }

        public override bool IsExpanded
        {
            get { return false; }
            set { throw new NotImplementedException(); }
        }
    }
}
