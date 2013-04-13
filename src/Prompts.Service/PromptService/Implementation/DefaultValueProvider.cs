namespace Prompts.Service.PromptService.Implementation
{
    public class DefaultValueProvider : IDefaultValueProvider
    {
        private readonly string _allMemberPrefix;

        public DefaultValueProvider(string allMemberPrefix)
        {
            _allMemberPrefix = allMemberPrefix;
        }

        public DefaultValue Get(string value, string label)
        {
            var isAllMember = value.StartsWith(_allMemberPrefix);

            var parsedValue = isAllMember ?  value.Replace(_allMemberPrefix, "") : value;

            return new DefaultValue(parsedValue, label, isAllMember);
        }
    }
}