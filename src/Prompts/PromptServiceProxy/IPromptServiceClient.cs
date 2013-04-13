using System;
using System.Collections.Generic;
using Prompts.Service.PromptService;

namespace Prompts.PromptServiceProxy
{
    public interface IPromptServiceClient
    {
        void GetPromptsForReportAsync(string path, Action<IEnumerable<PromptInfo>> callback, Action<string> errorCallback);
    }
}