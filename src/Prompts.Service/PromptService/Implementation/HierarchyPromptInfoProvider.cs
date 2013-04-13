using Prompts.Service.ReportExecution;

namespace Prompts.Service.PromptService.Implementation
{
    public class HierarchyPromptInfoProvider : IHierarchyPromptInfoProvider
    {
        private readonly IHierarchyPromptReportValidator _hierarchyValidator;
        private readonly IPromptTypeProvider _promptTypeProvider;
        private readonly IPromptLevelProvider _promptLevelProvider;
        private readonly IStrictDefaultValuesProvider _defaultValueProvider;

        public HierarchyPromptInfoProvider(
            IPromptTypeProvider promptTypeProvider,
            IPromptLevelProvider promptLevelProvider,
            IStrictDefaultValuesProvider defaultValueProvider,
            IHierarchyPromptReportValidator hierarchyPromptReportValidator)
        {
            _hierarchyValidator = hierarchyPromptReportValidator;
            _promptTypeProvider = promptTypeProvider;
            _promptLevelProvider = promptLevelProvider;
            _defaultValueProvider = defaultValueProvider;
        }

        public PromptInfo GetPromptInfo(GlobalPromptBaseReportInfo baseReportInfo, ReportParameter[] promptReportParameters)
        {
            _hierarchyValidator.Validate(baseReportInfo.Name, promptReportParameters);
            var promptLevel = _promptLevelProvider.GetPromptLevel(promptReportParameters[0]);
            var defaultValues = _defaultValueProvider.GetDefaultValues(promptLevel, baseReportInfo.ValueParameterDefaults);
            var promptType = _promptTypeProvider.GetPromptType(baseReportInfo.SelectionType);

            return new PromptInfo(
                baseReportInfo.Name, 
                baseReportInfo.Label,
                promptType, 
                promptLevel, 
                defaultValues);
        }
    }
}