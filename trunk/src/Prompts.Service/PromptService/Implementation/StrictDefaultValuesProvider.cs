using System.Collections.Generic;
using System.Linq;

namespace Prompts.Service.PromptService.Implementation
{
    public class StrictDefaultValuesProvider : IStrictDefaultValuesProvider
    {
        private readonly IDefaultValueProvider _defaultValueProvider;

        public StrictDefaultValuesProvider(IDefaultValueProvider defaultValueProvider)
        {
            _defaultValueProvider = defaultValueProvider;
        }

        public IEnumerable<DefaultValue> GetDefaultValues(PromptLevel promptLevel, IEnumerable<string> defaultValues)
        {
            var matchingAvailableItems = new List<DefaultValue>();

            var defaults = new List<DefaultValue>();

            foreach (var defaultValue in defaultValues)
            {
                var @default = _defaultValueProvider.Get(defaultValue, string.Empty);
                defaults.Add(@default);
            }

            foreach (var valueParameterDefault in defaults)
            {
                var localValueParameterDefault = valueParameterDefault;
                var validValue = promptLevel.AvailableItems.Where(i => i.Value == localValueParameterDefault.Value).SingleOrDefault();
                    
                if(validValue != null)
                {
                    matchingAvailableItems.Add(valueParameterDefault);
                }
            }

            return matchingAvailableItems;
        }
    }
}