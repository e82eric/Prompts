using System;
using System.Collections.Generic;
using Moq;
using Prompts.PromptServiceProxy;
using Prompts.Service.PromptService;

namespace Test.Prompts.Infrastructure.Fakes
{
    internal class FakePromptServiceClient
    {
        private readonly Mock<IPromptServiceClient> _promptServiceClient;
        private readonly Dictionary<string, Tuple<Action<IEnumerable<PromptInfo>>,Action<string>>> _callbackDictionary;

        public FakePromptServiceClient(Mock<IPromptServiceClient> promptServiceClient)
        {
            _promptServiceClient = promptServiceClient;
            _callbackDictionary = new Dictionary<string, Tuple<Action<IEnumerable<PromptInfo>>,Action<string>>>();
        }

        public void SetupGetPromptsAsync(string reportPath)
        {
            var setup = _promptServiceClient.Setup(c => c.GetPromptsForReportAsync(
                reportPath
                , It.IsAny<Action<IEnumerable<PromptInfo>>>()
                , It.IsAny<Action<string>>()));

            setup.Callback<string, Action<IEnumerable<PromptInfo>>,Action<string>>(
                (s, callback, errorCallback) =>
                    {
                        if (!_callbackDictionary.ContainsKey(reportPath))
                        {
                            _callbackDictionary.Add(reportPath, new Tuple<Action<IEnumerable<PromptInfo>>, Action<string>>(callback, errorCallback));
                        }
                    });
        }

        public void RaiseGetPromptsCompleted(IEnumerable<PromptInfo> promptInfos, string reportPath)
        {
            Tuple<Action<IEnumerable<PromptInfo>>,Action<string>> callbacks;

            _callbackDictionary.TryGetValue(reportPath, out callbacks);

            callbacks.Item1(promptInfos);
        }

        public void RaiseGetPromptsCompletedWithError(string errorMessage, string reportPath)
        {
            Tuple<Action<IEnumerable<PromptInfo>>, Action<string>> callbacks;

            _callbackDictionary.TryGetValue(reportPath, out callbacks);

            callbacks.Item2(errorMessage);
        }
    }
}
