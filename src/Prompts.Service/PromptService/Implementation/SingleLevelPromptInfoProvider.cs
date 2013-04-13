using Prompts.Service.ReportExecution;

namespace Prompts.Service.PromptService.Implementation
{
    public class SingleLevelPromptInfoProvider : ISingleLevelPromptInfoProvider
    {
        private readonly IPromptTypeProvider _promptTypeProvider;
        private readonly IPromptLevelProvider _promptLevelProvider;
        private readonly IStrictDefaultValuesProvider _defaultValueProvider;

        public SingleLevelPromptInfoProvider(IPromptTypeProvider promptTypeProvider
            , IPromptLevelProvider promptLevelProvider
            , IStrictDefaultValuesProvider defaultValueProvider)
        {
            _defaultValueProvider = defaultValueProvider;
            _promptLevelProvider = promptLevelProvider;
            _promptTypeProvider = promptTypeProvider;
        }

        public virtual PromptInfo GetPromptInfo(GlobalPromptBaseReportInfo baseReportInfo, ReportParameter promptReportParameter)
        {
            var promptLevel = _promptLevelProvider.GetPromptLevel(promptReportParameter);
            var defaultValues = _defaultValueProvider.GetDefaultValues(promptLevel, baseReportInfo.ValueParameterDefaults);
            var promptType = _promptTypeProvider.GetPromptType(baseReportInfo.SelectionType);

            return new PromptInfo(baseReportInfo.Name, baseReportInfo.Label, promptType, promptLevel, defaultValues);
        }
    }
}