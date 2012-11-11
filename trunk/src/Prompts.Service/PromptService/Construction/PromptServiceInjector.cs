using Prompts.Service.PromptService.Implementation;
using Prompts.Service.Properties;

namespace Prompts.Service.PromptService.Construction
{
    public class PromptServiceInjector
    {
        private static readonly IReportExecutionService ReportExecutionService = ReportExecutionServiceInjector.Inject();

        public static PromptService Inject()
        {
            return new PromptService(InjectBaseReportParameterService(), InjectBaseReportInterpreter());
        }

        private static IBaseReportInterpreter<PromptInfo> InjectBaseReportInterpreter()
        {
            return new BaseReportInterpreter<PromptInfo>(
                InjectGlobalPromptProvider()
                , InjectEmptyPromptInfoProvider());
        }

        private static IEmbeddedPromptProvider<PromptInfo> InjectEmptyPromptInfoProvider()
        {
            return new EmbeddedPromptInfoProvider(
                InjectStrictDefaultValueProvider(),
                InjectSingleLevelPromptLevelProvider(),
                InjectEmptyPromptDefaultValueProvider());
        }

        private static IEmptyPromptDefaultValueProvider InjectEmptyPromptDefaultValueProvider()
        {
            return new EmptyPromptDefaultValueProvider(InjectDefaultValueProvider());
        }

        private static IGlobalPromptProvider<PromptInfo> InjectGlobalPromptProvider()
        {
            return new GlobalPromptInfoService(
                PromptReportParameterServiceInjector.Inject()
                , InjectGlobalPromptInfoProvider()
                , InjectGlobalPromptBaseReportInfoMapper());
        }

        private static IGlobalPromptBaseReportInfoMapper InjectGlobalPromptBaseReportInfoMapper()
        {
            return new GlobalPromptBaseReportInfoMapper();
        }

        private static IGlobalPromptInfoProvider InjectGlobalPromptInfoProvider()
        {
            return new GlobalPromptInfoProvider(
                InjectSingleLevelPromptInfoProvider()
                , InjectHierarchyPromptInfoProvider()
                , InjectCasscadingPromptInfoProvider()
                , InjectRecursiveHierarchyPromptInfoProvider()
                , Settings.Default.RecursiveTreePrefix);
        }

        private static RecursiveHierarchyPromptInfoProvider InjectRecursiveHierarchyPromptInfoProvider()
        {
            return new RecursiveHierarchyPromptInfoProvider(
                new RecursiveHierarchyPromptTypeProvider(),
                InjectStrictDefaultValueProvider(),
                new RecursiveHierarchyValidator());
        }

        private static ICasscadingPromptInfoProvider InjectCasscadingPromptInfoProvider()
        {
            return new CasscadingPromptInfoProvider(
                InjectCasscadingPromptTypeProvider(),
                InjectEmptyPromptLevelProvider(),
                InjectCascadingSearchDefaultVaueProvider(),
                InjectCasscadingSearchValidator());
        }

        private static ICasscadingSearchValidator InjectCasscadingSearchValidator()
        {
            return new CasscadingSearchValidator();
        }

        private static ICascadingSearchDefaultValueProvider InjectCascadingSearchDefaultVaueProvider()
        {
            return new CascadingSearchDefaultValueProvider(
                PromptReportParameterServiceInjector.Inject(),
                InjectDefaultValueProvider());
        }

        private static ICascadingSearchPromptLevelProvider InjectEmptyPromptLevelProvider()
        {
            return new CascadingSearchPromptLevelProvider();
        }

        private static IPromptTypeProvider InjectCasscadingPromptTypeProvider()
        {
            return new CasscadingSearchPromptTypeProvider();
        }

        private static IHierarchyPromptInfoProvider InjectHierarchyPromptInfoProvider()
        {
            return new HierarchyPromptInfoProvider(
                InjectHierarchyPromptTypeProvider(),
                InjectHierarchyPromptLevelProvider(),
                InjectStrictDefaultValueProvider(),
                InjectHierarchyPromptReportValidator());
        }

        private static IHierarchyPromptReportValidator InjectHierarchyPromptReportValidator()
        {
            return new HierarchyPromptReportValidator();
        }

        private static IPromptLevelProvider InjectHierarchyPromptLevelProvider()
        {
            return new HierarchyPromptLevelProvider();
        }

        private static IPromptTypeProvider InjectHierarchyPromptTypeProvider()
        {
            return new HierarchyPromptTypeProvider();
        }

        private static ISingleLevelPromptInfoProvider InjectSingleLevelPromptInfoProvider()
        {
            return new SingleLevelPromptInfoProvider(
                InjectSingleLevelPromptTypeProvider(),
                InjectSingleLevelPromptLevelProvider(),
                InjectStrictDefaultValueProvider());
        }

        private static IStrictDefaultValuesProvider InjectStrictDefaultValueProvider()
        {
            return new StrictDefaultValuesProvider(InjectDefaultValueProvider());
        }

        private static DefaultValueProvider InjectDefaultValueProvider()
        {
            return new DefaultValueProvider(Settings.Default.AllMemberPrefix);
        }

        private static IPromptLevelProvider InjectSingleLevelPromptLevelProvider()
        {
            return new SingleLevelPromptLevelProvider();
        }

        private static IPromptTypeProvider InjectSingleLevelPromptTypeProvider()
        {
            return new SingleLevelPromptTypeProvider();
        }

        private static IBaseReportParameterService InjectBaseReportParameterService()
        {
            return new BaseReportParameterService(ReportExecutionService);
        }
    }
}