namespace Prompts.Service.PromptService.Implementation
{
    public class IntegratedPromptReportNameParser : IPromptReportNameParser
    {
        public string Parse(string promptReportName)
        {
            var nameToReturn = promptReportName;

            if (promptReportName.StartsWith("A_"))
            {
                var promptNameWithoutAliasPrefix = promptReportName.Remove(0, 2);
                var lastIndexOfFinalUnderscore = promptNameWithoutAliasPrefix.LastIndexOf('_');
                nameToReturn = promptNameWithoutAliasPrefix.Remove(lastIndexOfFinalUnderscore, promptNameWithoutAliasPrefix.Length - lastIndexOfFinalUnderscore);
            }

            return string.Format("{0}.rdl", nameToReturn);
        }
    }
}