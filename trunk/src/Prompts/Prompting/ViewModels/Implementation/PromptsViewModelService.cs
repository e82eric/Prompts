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
using System;
using System.Collections.Generic;
using Prompts.Prompting.Construction;
using Prompts.PromptServiceProxy;
using Prompts.Service.PromptService;

namespace Prompts.Prompting.ViewModels.Implementation
{
    public class PromptsViewModelService : IPromptsViewModelService
    {
        private readonly IPromptsViewModelBuilder _promptsViewModelBuilder;
        private readonly IPromptServiceClient _promptService;
        private readonly Dictionary<string, Tuple<Action<IEnumerable<IPrompt>>, Action<string>>> _callbacks;

        public PromptsViewModelService(
            IPromptsViewModelBuilder promptsViewModelBuilder,
            IPromptServiceClient promptService)
        {
            _promptService = promptService;
            _promptsViewModelBuilder = promptsViewModelBuilder;
            _callbacks = new Dictionary<string, Tuple<Action<IEnumerable<IPrompt>>, Action<string>>>();
        }

        public void GetPromptViewModels(
            string reportName,
            Action<IEnumerable<IPrompt>> result,
            Action<string> errorCallback)
        {
            var tuple = new Tuple<Action<IEnumerable<IPrompt>>, Action<string>>(result, errorCallback);

            _callbacks.Add(
                reportName, 
                tuple);

            _promptService.GetPromptsForReportAsync(
                reportName, 
                r => OnGetPromptsForReportCompleted(r, reportName),
                r => OnGetPromptsForReportError(r, reportName));
        }

        public void CancelGetPromptViewModels(string reportPath)
        {
            _callbacks.Remove(reportPath);
        }

        private void OnGetPromptsForReportCompleted(
            IEnumerable<PromptInfo> response, 
            string reportPath)
        {
            Tuple<Action<IEnumerable<IPrompt>>, Action<string>> callbackTuple;
            _callbacks.TryGetValue(reportPath, out callbackTuple);

            if(callbackTuple != null)
            {
                var promptCollection = _promptsViewModelBuilder.BuildFrom(reportPath, response);

                callbackTuple.Item1(promptCollection);
                _callbacks.Remove(reportPath);
            }
        }

        private void OnGetPromptsForReportError(
            string response,
            string reportPath)
        {
            Tuple<Action<IEnumerable<IPrompt>>, Action<string>> callbackTuple;
            _callbacks.TryGetValue(reportPath, out callbackTuple);

            if (callbackTuple != null)
            {
                callbackTuple.Item2(response);
                _callbacks.Remove(reportPath);
            }
        }
    }
}