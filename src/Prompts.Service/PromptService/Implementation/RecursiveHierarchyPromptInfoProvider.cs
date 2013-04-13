using Prompts.Service.ReportExecution;

namespace Prompts.Service.PromptService.Implementation
{
    public class RecursiveHierarchyPromptInfoProvider : IHierarchyPromptInfoProvider
    {
        private readonly IHierarchyPromptReportValidator _hierarchyPromptReportValidator;
        private readonly IPromptTypeProvider _promptTypeProvider;
        private readonly IStrictDefaultValuesProvider _defaultValueProvider;

        public RecursiveHierarchyPromptInfoProvider(
            IPromptTypeProvider promptTypeProvider,
            IStrictDefaultValuesProvider defaultValueProvider,
            IHierarchyPromptReportValidator hierarchyPromptReportValidator) 
        {
            _hierarchyPromptReportValidator = hierarchyPromptReportValidator;
            _defaultValueProvider = defaultValueProvider;
            _promptTypeProvider = promptTypeProvider;
        }

        public PromptInfo GetPromptInfo(GlobalPromptBaseReportInfo baseReportInfo, ReportParameter[] promptReportParameters)
        {
            _hierarchyPromptReportValidator.Validate(baseReportInfo.Name, promptReportParameters);
            var promptLevel = new PromptLevel(promptReportParameters[0].Name, promptReportParameters[1].ValidValues, true);
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