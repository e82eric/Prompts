using System.Collections.Generic;
using Prompts.Service.ReportExecution;

namespace Prompts.Service.PromptService.Implementation
{
    public class BaseReportInterpreter<T> : IBaseReportInterpreter<T>
    {
        private readonly IGlobalPromptProvider<T> _globalPromptProvider;
        private readonly IEmbeddedPromptProvider<T> _embeddedPromptProvider;

        public BaseReportInterpreter(IGlobalPromptProvider<T> globalPromptProvider, IEmbeddedPromptProvider<T> embeddedPromptProvider)
        {
            _globalPromptProvider = globalPromptProvider;
            _embeddedPromptProvider = embeddedPromptProvider;
        }

        public IEnumerable<T> Get(ReportParameter[] baseReportParameters)
        {
            var prompts = new List<T>();

            if (baseReportParameters == null)
            {
                return new T[] { };
            }
            for (var i = 0; i < baseReportParameters.Length; i++)
            {
                if (baseReportParameters[i].Prompt != string.Empty)
                {
                    if (baseReportParameters[i].ValidValues == null)
                    {
                        if (i + 1 == baseReportParameters.Length)
                        {
                            var prompt = _embeddedPromptProvider.Get(baseReportParameters[i]);
                            prompts.Add(prompt);
                        }
                        else if (baseReportParameters[i + 1].Name.StartsWith(baseReportParameters[i].Name) &&
                                 baseReportParameters[i + 1].ValidValues == null)
                        {
                            var valueParameter = baseReportParameters[i];
                            i++;
                            var labelParameter = baseReportParameters[i];
                            var prompt = _globalPromptProvider.Get(valueParameter, labelParameter);
                            prompts.Add(prompt);
                        }
                        else
                        {
                            var prompt = _embeddedPromptProvider.Get(baseReportParameters[i]);
                            prompts.Add(prompt);
                        }
                    }
                    else
                    {
                        var prompt = _embeddedPromptProvider.Get(baseReportParameters[i]);
                        prompts.Add(prompt);
                    }
                }
            }

            return prompts;
        }
    }
}