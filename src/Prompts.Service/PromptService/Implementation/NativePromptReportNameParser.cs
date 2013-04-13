namespace Prompts.Service.PromptService.Implementation
{
    public class NativePromptReportNameParser : IPromptReportNameParser
    {
        public string Parse(string promptReportName)
        {
            if(promptReportName.StartsWith("A_"))
            {
                var promptNameWithoutAliasPrefix = promptReportName.Remove(0, 2);
                var lastIndexOfFinalUnderscore = promptNameWithoutAliasPrefix.LastIndexOf('_');
                return promptNameWithoutAliasPrefix.Remove(lastIndexOfFinalUnderscore, promptNameWithoutAliasPrefix.Length - lastIndexOfFinalUnderscore);
            }

            return promptReportName;
        }
    }
}