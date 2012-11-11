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
using System.Windows;
using Prompts.Prompting.Model;
using Prompts.PromptServiceProxy;
using Prompts.ReportCatalog.Model;
using ServiceStack.ServiceClient.Web;

namespace Prompts
{
    public static class ServiceInjector
    {
        public static T Inject<T>() where T : class
        {
            if (typeof (T) == typeof (IChildPromptLevelServiceClient))
            {
                return (T)(object)new ChildPromptLevelServiceClient(GetJsonRestClient(), "/prompts/child_items");
            }
            if (typeof (T) == typeof (IPromptSelectionServiceClient))
            {
                return (T)(object)new PromptSelectionServiceClient(GetJsonRestClient(), "/prompts/set_prompt_selections");
            }
            if(typeof(T) == typeof(IReportCatalogServiceClient))
            {
                return (T)(object)new ReportCatalogServiceClient(GetJsonRestClient(), "/reports");
            }
            if(typeof(T) == typeof(IPromptServiceClient))
            {
                return (T) (object) new PromptServiceClient(GetJsonRestClient(), "/prompts");
            }

            throw new Exception();
        }

        private static JsonRestClientAsync GetJsonRestClient()
        {
            string uri;
            if (Application.Current.Host.Source != null)
            {
                var server = Application.Current.Host.Source.Host;
                var port = Application.Current.Host.Source.Port;
                uri = string.Format("http://{0}:{1}/prompts.service/api", server, port);
            }
            else
            {
                throw new Exception();
            }

            return new JsonRestClientAsync(uri);
        }
    }
}