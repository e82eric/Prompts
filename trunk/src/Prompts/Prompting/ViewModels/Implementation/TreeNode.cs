using System.Collections.ObjectModel;
using Prompts.Service.ReportExecution;

namespace Prompts.Prompting.ViewModels.Implementation
{
    public abstract class TreeNode : SearchablePromptItem, ITreeNode
    {
        private ObservableCollection<ITreeNode> _children;
        private readonly ITreeNode _parent;
        private readonly bool _isEnabled;

        protected TreeNode(
            string promptName
            , string parameterName
            , ValidValue validValue
            , ObservableCollection<ITreeNode> children, ITreeNode parent, bool isEnabled, bool isDefaultAll)
            : base(
                promptName
                , parameterName
                , validValue
                , isDefaultAll)
        {
            _children = children;
            _parent = parent;
            _isEnabled = isEnabled;
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                RaisePropertyChanged("IsSelected");
            }
        }

        public ITreeNode Parent
        {
            get { return _parent; }
        }

        public ObservableCollection<ITreeNode> Children
        {
            get { return _children; }
            internal set
            {
                _children = value;
                RaisePropertyChanged("Children");
            }
        }

        public abstract bool IsExpanded { get; set; }

        public ParameterValue GetParameterValue()
        {
            var parameterValue = new ParameterValue {Name = ParameterName, Value = Value};
            return parameterValue;
        }

        public ObservableCollection<ParameterValue> GetParameterValueHierarchy()
        {
            var parameterValues = new ObservableCollection<ParameterValue>();

            ITreeNode currentNode = this;

            while (currentNode != null)
            {
                parameterValues.Add(currentNode.GetParameterValue());
                currentNode = currentNode.Parent;
            }

            return parameterValues;
        }
    }
}