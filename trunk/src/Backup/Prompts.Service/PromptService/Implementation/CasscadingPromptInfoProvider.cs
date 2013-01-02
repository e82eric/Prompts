using Prompts.Service.ReportExecution;

namespace Prompts.Service.PromptService.Implementation
{
    public class CasscadingPromptInfoProvider : ICasscadingPromptInfoProvider
    {
        private readonly IPromptTypeProvider _promptTypeProvider;
        private readonly ICascadingSearchPromptLevelProvider _promptLevelProvider;
        private readonly ICasscadingSearchValidator _promptReportValidator;
        private readonly ICascadingSearchDefaultValueProvider _defaultValueProvider;

        public CasscadingPromptInfoProvider(IPromptTypeProvider promptTypeProvider
            , ICascadingSearchPromptLevelProvider promptLevelProvider
            , ICascadingSearchDefaultValueProvider defaultValueProvider
            , ICasscadingSearchValidator promptReportValidator)
        {
            _defaultValueProvider = defaultValueProvider;
            _promptReportValidator = promptReportValidator;
            _promptLevelProvider = promptLevelProvider;
            _promptTypeProvider = promptTypeProvider;
        }

        public PromptInfo GetPromptInfo(
            GlobalPromptBaseReportInfo baseReportInfo, 
            ReportParameter searchParameter, 
            ReportParameter resultParameter)
        {
            _promptReportValidator.Validate(baseReportInfo.Name, searchParameter, resultParameter);

            var name = baseReportInfo.Name;
            var label = baseReportInfo.Label;
            var promptType = _promptTypeProvider.GetPromptType(baseReportInfo.SelectionType);
            
            var defaultValues = _defaultValueProvider.Get(
                name, 
                searchParameter.Name, 
                baseReportInfo.ValueParameterDefaults, 
                baseReportInfo.LabelParameterDefaults);
            
            var promptLevel = _promptLevelProvider.GetPromptLevel(searchParameter.Name, defaultValues);

            return new PromptInfo(name, label, promptType, promptLevel, defaultValues);
        }
    }
}