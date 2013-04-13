// Copyright (c) 2010. Eric Nelson  
// https://code.google.com/p/prompts/
// All rights reserved.
//     
// Redistribution and use in source and binary forms,   
// with or without modification, are permitted provided   
// that the following conditions are met:    
// * Redistributions of source code must retain the   
// above copyright notice, this list of conditions and   
// the following disclaimer.    
// * Redistributions in binary form must reproduce   
// the above copyright notice, this list of conditions   
// and the following disclaimer in the documentation   
// and/or other materials provided with the distribution.    
// * Neither the name of Eric Nelson nor the   
// names of its contributors may be used to endorse   
// or promote products derived from this software   
// without specific prior written permission.    
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND   
// CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES,   
// INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF   
// MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE   
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR   
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,   
// SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING,   
// BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR   
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS   
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,   
// WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING   
// NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE   
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF   
// SUCH DAMAGE.
// 
//     
// [This is the BSD license, see  
// http://www.opensource.org/licenses/bsd-license.php]  
using System.Collections.ObjectModel;
using System.Linq;
using Prompts.Service.PromptService;
using Prompts.Service.ReportExecution;

namespace Prompts.Prompting.ViewModels.Implementation
{
    public class SingleSelectPrompt<T> : Prompt where T : IPromptItem
    {
        private IPromptItem _selectedItem;

        public SingleSelectPrompt(string name, string label, ObservableCollection<T> promptItems, IPromptItem defaultSelection)
            : base(name, label)
        {
            AvailableItems = promptItems;
            SelectedItem = defaultSelection;
            SetReadyForReportExecutionChangedIfNeeded();
        }

        private ObservableCollection<T> _availableItems;
        public ObservableCollection<T> AvailableItems
        {
            get { return _availableItems; }
            set
            {
                _availableItems = value;
                RaisePropertyChanged("AvailableItems");
            }
        }

        public IPromptItem SelectedItem
        {
            get { return _selectedItem; }
            set { OnItemSelected(value);}
        }

        protected virtual void OnItemSelected(IPromptItem value)
        {
            _selectedItem = FindSelectedItemInEnumeration(value);
            SetReadyForReportExecutionChangedIfNeeded();
            RaisePropertyChanged("SelectedItem");
        }

        protected virtual IPromptItem FindSelectedItemInEnumeration(IPromptItem itemToSelect)
        {
            if (itemToSelect == null)
            {
                return null;
            }
            return AvailableItems.Where(i => i.Equals(itemToSelect)).Single();
        }

        public override PromptSelectionInfo ToSelectionInfo()
        {
            var validValue = new ValidValue { Value = SelectedItem.Value, Label = SelectedItem.Label };
            var validValueCollection = new ObservableCollection<ValidValue>(new[] { validValue });
            return new PromptSelectionInfo
            {
                PromptName = Name,
                Selections = validValueCollection,
            };
        }

        protected override bool EvaluateReadyForReportExecution()
        {
            return SelectedItem != null ? true : false;
        }
    }
}