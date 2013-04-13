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
using Prompts.Infastructure;
using Prompts.ReportCatalog.Model;
using Prompts.Service.PromptService;

namespace Prompts.ReportRendering.ViewModel
{
    public class ReportViewModel : StatefulViewModel, IReportViewModel
    {
        private string _url;
        private readonly string _serverName;

        public ReportViewModel(
            CatalogItemInfo catalogItemInfo
            , ObservableCollection<PromptSelectionInfo> selectionInfos
            , IReportExecutionService reportExecutionService
            , string serverName)
        {
            _serverName = serverName;
            _url = string.Empty;
            reportExecutionService.Render(
                catalogItemInfo, 
                selectionInfos, 
                OnRender, 
                OnError);
            State = ViewModelState.Loading;
        }

        private void OnError(string obj)
        {
            ErrorMessage = obj;
            State = ViewModelState.Error;
        }

        private void OnRender(string executionId)
        {
            const string format = "http://{0}/Prompts.Service/ReportViewer.aspx?ExecutionId={1}";
            var url = string.Format(format, _serverName, executionId);
            Url = url;
            State = ViewModelState.Loaded;
        }

        public string Url
        {
            get { return _url; }
            private set
            {
                _url = value;
                RaisePropertyChanged("Url");
            }
        }
    }
}