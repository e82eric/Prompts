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
using System.Windows.Input;
using Prompts.Infastructure;
using Prompts.Prompting.ViewModels.Search;

namespace Prompts.Prompting.ViewModels.Implementation
{
    public class MultiSelectCasscadingSearch : MultiSelectPrompt<ISearchablePromptItem>, IMultiSelectPrompt
    {
        private ViewModelState _state;
        private readonly IAsynchronousSearchService _asynchronousSearchService;

        public MultiSelectCasscadingSearch(
            string label,
            string name,
            IAsynchronousSearchService asynchronousSearchService,
            ObservableCollection<ISearchablePromptItem> availableItems,
            ObservableCollection<ISearchablePromptItem> defaultSelections)
            : base(label, name, defaultSelections)
        {
            _asynchronousSearchService = asynchronousSearchService;
            AvailableItems = availableItems;

            if(AvailableItems.Count > 0)
            {
                State = ViewModelState.Loaded;
            }
        }

        public string SearchString { get; set; }

        public string ErrorMessage { get; set; }

        public ICommand Search
        {
            get { return new RelayCommand(OnSearch); }
        }

        protected void OnSearch()
        {
            State = ViewModelState.Loading;

            _asynchronousSearchService.Search(SearchString, OnSearchComplete, OnSearchError);
        }

        private void OnSearchError(string errorMessage)
        {
            AvailableItems = new ObservableCollection<ISearchablePromptItem>();
            State = ViewModelState.Error;
            ErrorMessage = errorMessage;
            RaisePropertyChanged("ErrorMessage");
        }

        private void OnSearchComplete(ObservableCollection<ISearchablePromptItem> searchResult)
        {
            AvailableItems = searchResult;
            State = ViewModelState.Loaded;
        }
        
        public ViewModelState State
        {
            get { return _state; }
            private set
            {
                _state = value;
                RaisePropertyChanged("State");
            }
        }
    }
}