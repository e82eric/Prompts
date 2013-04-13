using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Prompts.Infastructure;
using Prompts.Service.PromptService;
using Prompts.Service.ReportExecution;

namespace Prompts.Prompting.ViewModels.Implementation
{
    public class MultiSelectPrompt<T> : Prompt where T : class, IPromptItem
    {
        private ObservableCollection<T> _selectedItems;
        private ObservableCollection<T> _availableItems;

        public MultiSelectPrompt(
            string label, 
            string name, 
            ObservableCollection<T> selectedItems)
            : base(name, label)
        {
            _selectedItems = selectedItems;
            SetReadyForReportExecutionChangedIfNeeded();
            SelectItems = new RelayCommand(OnSelectItems);
            DeSelectItems = new RelayCommand(OnDeselectItems);
        }

        private void OnSelectItems()
        {
            if(SelectedAvailableItems == null)
            {
                return;
            }

            IEnumerable<T> selectedItemsToRemove = null;

            if (SelectedItems != null)
            {
                selectedItemsToRemove = SelectedItems.Where(i => i.IsDefaultAll).ToArray();
            }

            if (selectedItemsToRemove != null)
            {
                foreach (var selectedItem in selectedItemsToRemove)
                {
                    if (selectedItem.IsDefaultAll)
                    {
                        OnDeSelectItem(selectedItem);
                    }
                }
            }

            foreach (var itemToSelect in SelectedAvailableItems)
            {
                OnSelectItem(itemToSelect);
            }
        }

        private void OnDeselectItems()
        {
            if(SelectedSelectedItems == null)
            {
                return;
            }

            foreach (var itemToDeSelect in SelectedSelectedItems)
            {
                OnDeSelectItem(itemToDeSelect);
            }
        }

        public ICommand SelectItems { get; private set; }
        public ICommand DeSelectItems { get; private set; }

        public virtual ObservableCollection<T> SelectedItems
        {
            get { return _selectedItems; }
            set
            {
                _selectedItems = value;
                RaisePropertyChanged("SelectedItems");
            }
        }

        private void OnSelectItem(T searchablePromptItem)
        {
            

            if (searchablePromptItem == null)
            {
                return;
            }

            if (!SelectedItems.Contains(searchablePromptItem))
            {
                SelectedItems.Add(searchablePromptItem);
                SetReadyForReportExecutionChangedIfNeeded();
            }

            SetReadyForReportExecutionChangedIfNeeded();
        }

        private void OnDeSelectItem(T searchablePromptItem)
        {
            if (searchablePromptItem == null)
            {
                return;
            }

            SelectedItems.Remove(searchablePromptItem);
            SetReadyForReportExecutionChangedIfNeeded();
        }

        protected override bool EvaluateReadyForReportExecution()
        {
            return SelectedItems.Count.Equals(0) ? false : true;
        }

        public virtual IEnumerable<T> SelectedAvailableItems { get; set; }
        public IEnumerable<T> SelectedSelectedItems { get; set; }

        public ObservableCollection<T> AvailableItems
        {
            get { return _availableItems; }
            set
            {
                _availableItems = value;
                RaisePropertyChanged("AvailableItems");
            }
        }

        public override PromptSelectionInfo ToSelectionInfo()
        {
            var validValues = new List<ValidValue>();

            foreach (var selectedItem in SelectedItems)
            {
                var validValue = new ValidValue
                    {
                        Label = selectedItem.Label,
                        Value = selectedItem.Value
                    };
                validValues.Add(validValue);
            }

            var validValueCollection = new ObservableCollection<ValidValue>(validValues);

            return new PromptSelectionInfo
                {
                    PromptName = Name,
                    Selections = validValueCollection,
                };
        }
    }
}