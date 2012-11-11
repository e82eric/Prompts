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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Prompts.Infastructure;
using Prompts.ReportCatalog.Model;
using Prompts.ReportRendering.ViewModel;
using Prompts.Service.PromptService;

namespace Prompts.Prompting.ViewModels.Implementation
{
    public class PromptsViewModel : StatefulViewModel, IPromptsViewModel
    {
        private ObservableCollection<IPrompt> _prompts;
        private readonly RelayCommand _executeReport;
        private bool _readyForReportExecution;
        private readonly IPromptsViewModelService _promptsViewModelService;
        private CatalogItemInfo _catalogItemInfo;
        private readonly IReportRenderer _reportRenderer;

        public PromptsViewModel(IPromptsViewModelService promptsViewModelService, IReportRenderer reportRenderer)
        {
            _reportRenderer = reportRenderer;
            _catalogItemInfo = new CatalogItemInfo {Name = string.Empty};
            _prompts = new ObservableCollection<IPrompt>();
            _promptsViewModelService = promptsViewModelService;
            _executeReport = new RelayCommand(OnExeucteReport, ValidateAllPromptsAreReadyForReportExecution);

            MoveNext = new RelayCommand(OnMoveNext, () => _canMoveNext);
            MovePrevious = new RelayCommand(OnMovePrevious,() => _canMovePrevious);
        }

        private void EvaluateCanMovePrevious()
        {
            var newCanMovePrevious = Prompts.IndexOf(SelectedPrompt) == 0 ? false : true;
            if(newCanMovePrevious != _canMovePrevious)
            {
                _canMovePrevious = newCanMovePrevious;
                MovePrevious.RaiseCanExecuteChanged();
            }
        }

        private void EvaluateCanMoveNext()
        {
            if(Prompts.Count != 0)
            {
                var newCanMoveNext = Prompts.IndexOf(SelectedPrompt) == Prompts.Count -1 ? false : true;
                if (newCanMoveNext != _canMoveNext)
                {
                    _canMoveNext = newCanMoveNext;
                    MoveNext.RaiseCanExecuteChanged();
                }
            }
        }

        private void OnMovePrevious()
        {
            SelectedPrompt = Prompts[Prompts.IndexOf(SelectedPrompt) - 1];
            EvaluateCanMovePrevious();
            EvaluateCanMoveNext();
        }

        private void OnMoveNext()
        {
            SelectedPrompt = Prompts[Prompts.IndexOf(SelectedPrompt) + 1];
            EvaluateCanMovePrevious();
            EvaluateCanMoveNext();
        }

        private void OnErrorGettingPrompts(string errorMessage)
        {
            State = ViewModelState.Error;
            ErrorMessage = errorMessage;
        }

        private void OnGetPromptCollectionCompleted(IEnumerable<IPrompt> prompts)
        {
            foreach (var prompt in prompts)
            {
                prompt.PropertyChanged += OnPromptPropertyChanged;
            }

            Prompts = new ObservableCollection<IPrompt>(prompts);
            SelectedPrompt = prompts.FirstOrDefault();
            EvaluateCanMoveNext();
            State = ViewModelState.Loaded;
            var readyForReportExecution = ValidateAllPromptsAreReadyForReportExecution();
            SetReadyForReportExecution(readyForReportExecution);
        }

        private void OnPromptPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("ReadyForReportExecution"))
            {
                var newReadyForReportExectionFlag = ValidateAllPromptsAreReadyForReportExecution();
                SetReadyForReportExecution(newReadyForReportExectionFlag);
            }
        }

        private void SetReadyForReportExecution(bool value)
        {
            if (value != _readyForReportExecution)
            {
                _readyForReportExecution = value;
                _executeReport.RaiseCanExecuteChanged();
            }
        }

        private void OnExeucteReport()
        {
            var promptSelections = new ObservableCollection<PromptSelectionInfo>();

            foreach (var prompt in Prompts)
            {
                var promptSelection = prompt.ToSelectionInfo();
                promptSelections.Add(promptSelection);
            }

            _reportRenderer.AddReport(_catalogItemInfo, promptSelections);
        }

        private bool ValidateAllPromptsAreReadyForReportExecution()
        {
            if (State == ViewModelState.UnInitialized)
            {
                return false;
            }
            return _prompts.Where(p => p.ReadyForReportExecution.Equals(false)).Count() > 0 ? false : true;
        }

        private CatalogItemInfo CatalogItemInfo
        {
            get { return _catalogItemInfo; }
            set
            {
                _catalogItemInfo = value;
                RaisePropertyChanged("ReportName");
            }
        }

        public string ReportName
        {
            get { return CatalogItemInfo.Name; }
        }

        public ObservableCollection<IPrompt> Prompts
        {
            get { return _prompts; }
            private set
            {
                _prompts = value;
                RaisePropertyChanged("Prompts");
            }
        }

        public ICommand ExecuteReport
        {
            get { return _executeReport; }
        }

        public void ShowPromptsFor(CatalogItemInfo catalogItemInfo)
        {
            if(catalogItemInfo != CatalogItemInfo || State == ViewModelState.Error)
            {
                if(State == ViewModelState.Loading)
                {
                    _promptsViewModelService.CancelGetPromptViewModels(CatalogItemInfo.Path);
                }

                CatalogItemInfo = catalogItemInfo;
                State = ViewModelState.Loading;

                foreach (var prompt in Prompts)
                {
                    prompt.PropertyChanged -= OnPromptPropertyChanged;
                }

                if (!ErrorMessage.Equals(string.Empty))
                {
                    ErrorMessage = string.Empty;
                }

                _promptsViewModelService.GetPromptViewModels(
                    catalogItemInfo.Path, 
                    OnGetPromptCollectionCompleted,
                    OnErrorGettingPrompts);
            }
        }

        private IPrompt _selectedPrompt;
        private bool _canMovePrevious;
        private bool _canMoveNext;

        public IPrompt SelectedPrompt
        {
            get { return _selectedPrompt; }
            set
            {
                _selectedPrompt = value;
                RaisePropertyChanged("SelectedPrompt");
                EvaluateCanMoveNext();
                EvaluateCanMovePrevious();
            }
        }

        public RelayCommand MoveNext { get; private set; }

        public RelayCommand MovePrevious { get; private set; }
    }
}