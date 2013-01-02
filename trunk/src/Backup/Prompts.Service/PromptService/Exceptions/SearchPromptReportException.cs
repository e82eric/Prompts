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

namespace Prompts.Service.PromptService.Exceptions
{
    public class SearchPromptReportException : Exception
    {
        public SearchPromptReportException(string message) : base(message)
        {
        }

        public static void ThrowCannotBeSingleSelect(string promptName)
        {
            var message = string.Format ("Error building Global Prompt {0}, a Casscading Search cannot be single select", promptName);
            throw new SearchPromptReportException(message);
        }

        public static void ThrowSearchParametersValidValuesNotNull()
        {
            const string message = "Error building Search Prompt Report, first parameters valid values were not null";
            throw new SearchPromptReportException(message);
        }

        public static void ThrowSecondParameterIsNotDependentOnFirst()
        {
            const string message = "Error building Search Prompt Report, second parameter was not dependent on first";
            throw new SearchPromptReportException(message);
        }
    }
}