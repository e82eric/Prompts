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
using Prompts.Service.PromptService.Infrastructure;
using Prompts.Service.ReportExecution;

namespace Prompts.Service.PromptService.Implementation
{
    public class PromptReportParameterService : ReportingServicesClientBase, IPromptReportParameterService
    {
        private readonly IReportServerFolder _reportServerFolder;
        private readonly IReportExecutionService _reportExecutionService;
        private readonly IPromptReportNameParser _promptReportNameParser;

        public PromptReportParameterService(
            IReportServerFolder reportServerFolder,
            IReportExecutionService reportExecutionService,
            IPromptReportNameParser promptReportNameParser)
        {
            _reportServerFolder = reportServerFolder;
            _reportExecutionService = reportExecutionService;
            _promptReportNameParser = promptReportNameParser;
        }

        public ReportParameter[] GetParametersFor(string promptName)
        {
            var promptReportName = _promptReportNameParser.Parse(promptName);

            return TryReportingServicesOperation(() =>
            {
                var fullReportPath = _reportServerFolder.GetFullPathFor(promptReportName);
                return _reportExecutionService.LoadReport2(fullReportPath, null).Parameters;
            });
        }

        //TODO This needs tests
        public ReportParameter[] GetParametersFor(string promptName, ParameterValue[] parameterValues)
        {
            var promptReportName = _promptReportNameParser.Parse(promptName);

            return TryReportingServicesOperation(() =>
                {
                    var fullReportPath = _reportServerFolder.GetFullPathFor(promptReportName);
                    _reportExecutionService.LoadReport2(fullReportPath, null);
                    return _reportExecutionService.SetExecutionParameters2(parameterValues, null).Parameters;
                });
        }

        //TODO: This needs tests
        public ReportParameter[] GetParametersFor(string promptName, ParameterValue parameterValue)
        {
            return GetParametersFor(promptName, new[] { parameterValue });
        }
    }
}