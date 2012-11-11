using System.Collections.Generic;
using System.Linq;
using Prompts.Service.ReportExecution;

namespace Prompts.Service.PromptService.Implementation
{
    public class EmptyPromptDefaultValueProvider : IEmptyPromptDefaultValueProvider
    {
        private readonly IDefaultValueProvider _defaultValueProvider;

        public EmptyPromptDefaultValueProvider(IDefaultValueProvider defaultValueProvider)
        {
            _defaultValueProvider = defaultValueProvider;
        }

        public IEnumerable<DefaultValue> Get(ReportParameter parameter)
        {
            if(parameter.DefaultValues == null)
            {
                return new DefaultValue[] {};
            }

            return new []{ _defaultValueProvider.Get(parameter.DefaultValues.First(), string.Empty)};
        }
    }
}