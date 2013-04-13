using Prompts.Service.PromptService;
using Prompts.Service.ReportExecution;
using Test.Prompts.Service.Builders;

namespace Test.Prompts.Service.Infastructure
{
    class A
    {
        public static ValidValueBuilder ValidValue()
        {
            return new ValidValueBuilder();
        }

        public static ValidValue ValidValue(string value)
        {
            return ValidValue().WithValue(value).Build();
        }

        public static DefaultValueBuilder DefaultValue()
        {
            return new DefaultValueBuilder();
        }

        public static DefaultValue DefaultValue(string key)
        {
            return A.DefaultValue().WithValue(key).Build();
        }

        public static ReportParameter ReportParameter(string name)
        {
            return new ReportParameter { Name = name };
        }

        public static ReportParameterBuilder ReportParameter()
        {
            return new ReportParameterBuilder();
        }

        public static GlobalPromptBaseReportInfoBuilder GlobalPromptBaseReportInfo()
        {
            return new GlobalPromptBaseReportInfoBuilder();
        }

        public static PromptLevelBuilder PromptLevel()
        {
            return new PromptLevelBuilder();
        }

        public static ParameterValueBuilder ParameterValue()
        {
            return new ParameterValueBuilder();
        }

        public static PromptSelectionInfoBuilder PromptSelectionInfo()
        {
            return new PromptSelectionInfoBuilder();
        }

        public static PromptInfoBuilder PromptInfo()
        {
            return new PromptInfoBuilder();
        }

        public static CatalogItemBuilder CatalogItem()
        {
            return new CatalogItemBuilder();
        }

        public static CatalogItemInfoBuilder CatalogItemInfo()
        {
            return new CatalogItemInfoBuilder();
        }

        public static T[] Array<T>(params T[] array)
        {
            return array;
        }
    }
}
