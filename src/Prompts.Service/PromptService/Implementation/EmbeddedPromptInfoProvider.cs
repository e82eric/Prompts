using Prompts.Service.ReportExecution;

namespace Prompts.Service.PromptService.Implementation
{
    public class EmbeddedPromptInfoProvider : IEmbeddedPromptProvider<PromptInfo>
    {
        private readonly IStrictDefaultValuesProvider _strictDefaultValuesProvider;
        private readonly IPromptLevelProvider _promptLevelProvider;
        private readonly IEmptyPromptDefaultValueProvider _emptyPromptDefaultValueProvider;

        public EmbeddedPromptInfoProvider(
            IStrictDefaultValuesProvider strictDefaultValuesProvider,
            IPromptLevelProvider promptLevelProvider,
            IEmptyPromptDefaultValueProvider emptyPromptDefaultValueProvider)
        {
            _emptyPromptDefaultValueProvider = emptyPromptDefaultValueProvider;
            _promptLevelProvider = promptLevelProvider;
            _strictDefaultValuesProvider = strictDefaultValuesProvider;
        }

        public PromptInfo Get(ReportParameter baseReportParameter)
        {
            var promptLevel = _promptLevelProvider.GetPromptLevel(baseReportParameter);
            var defaultValues =
                baseReportParameter.ValidValues != null ? _strictDefaultValuesProvider.GetDefaultValues(
                        promptLevel,
                        baseReportParameter.DefaultValues ?? new string[] {})
                    : _emptyPromptDefaultValueProvider.Get(baseReportParameter);

            var promptType =
                baseReportParameter.ValidValues == null
                    ? PromptType.Empty
                    : baseReportParameter.MultiValue
                          ? PromptType.ShoppingCart
                          : PromptType.DropDown;

            return new PromptInfo(
                baseReportParameter.Name, 
                baseReportParameter.Prompt, 
                promptType, 
                promptLevel, 
                defaultValues);
        }
    }
}