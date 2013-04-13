using System.Collections.ObjectModel;
using Test.Prompts.Infrastructure.Builders;

namespace Test.Prompts.Infrastructure
{
    public class A
    {
        public static ValidValueBuilder ValidValue()
        {
            return new ValidValueBuilder();
        }

        public static DefaultValueBuilder DefaultValue()
        {
            return new DefaultValueBuilder();
        }

        public static ObservableCollection<T> ObservableCollection<T>(params T[] array)
        {
            return new ObservableCollection<T>(array);
        }

        public static PromptInfoBuilder PromptInfo()
        {
            return new PromptInfoBuilder();
        }

        public static ParameterValueBuilder ParameterValue()
        {
            return new ParameterValueBuilder();
        }

        public static PromptBuilder Prompt()
        {
            return new PromptBuilder();
        }

        public static PromptLevelBuilder PromptLevel()
        {
            return new PromptLevelBuilder();
        }

        public static CatalogItemInfoBuilder CatalogItemInfo()
        {
            return new CatalogItemInfoBuilder();
        }

        public static PromptSelectionInfoBuilder PromptSelectionInfo()
        {
            return new PromptSelectionInfoBuilder();
        }

        public static T[] Array<T>(params T[] array)
        {
            return array;
        }
    }
}
