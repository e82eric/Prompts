using System;
using System.Collections.ObjectModel;
using Prompts.Service.ReportExecution;

namespace Prompts.Prompting.ViewModels.Implementation
{
    public class LoadingTreeNode : ITreeNode
    {
        public string Label
        {
            get { return "Loading..."; }
        }

        public string Value
        {
            get { throw new NotImplementedException(); }
        }

        public string PromptName
        {
            get { throw new NotImplementedException(); }
        }

        public string ParameterName
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsDefaultAll
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public ITreeNode Parent
        {
            get { throw new NotImplementedException(); }
        }

        public ObservableCollection<ITreeNode> Children
        {
            get { return new ObservableCollection<ITreeNode>();}
        }

        public ParameterValue GetParameterValue()
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<ParameterValue> GetParameterValueHierarchy()
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled
        {
            get { return false; }
        }

        public bool IsSelected { get; set; }
    }
}
