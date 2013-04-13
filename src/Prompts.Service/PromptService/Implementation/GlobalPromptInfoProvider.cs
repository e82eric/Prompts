using System;
using System.Linq;
using Prompts.Service.PromptService.Exceptions;
using Prompts.Service.ReportExecution;

namespace Prompts.Service.PromptService.Implementation
{
    public class GlobalPromptInfoProvider : IGlobalPromptInfoProvider
    {
        private readonly ISingleLevelPromptInfoProvider _singleLevelPromptInfoProvider;
        private readonly IHierarchyPromptInfoProvider _hierarchyPromptInfoProvider;
        private readonly ICasscadingPromptInfoProvider _casscadingPromptInfoProvider;
        private readonly IHierarchyPromptInfoProvider _recursivePromptInfoProvider;
        private readonly string _recursiveHierarchyPrefix;

        public GlobalPromptInfoProvider(
            ISingleLevelPromptInfoProvider singleLevelPromptInfoProvider
            , IHierarchyPromptInfoProvider hierarchyPromptInfoProvider
            , ICasscadingPromptInfoProvider casscadingPromptInfoProvider
            , IHierarchyPromptInfoProvider recursivePromptInfoProvider
            , string recursiveHierarchyPrefix)
        {
            _recursiveHierarchyPrefix = recursiveHierarchyPrefix;
            _recursivePromptInfoProvider = recursivePromptInfoProvider;
            _singleLevelPromptInfoProvider = singleLevelPromptInfoProvider;
            _hierarchyPromptInfoProvider = hierarchyPromptInfoProvider;
            _casscadingPromptInfoProvider = casscadingPromptInfoProvider;
        }

        public PromptInfo GetPromptInfo(GlobalPromptBaseReportInfo baseReportInfo, ReportParameter[] promptReportParameters)
        {
            if (promptReportParameters == null)
            {
                const string messageFormat = "An error occured building Global Prompt '{0}', There were no parameters";
                var message = string.Format(messageFormat, baseReportInfo.Name);
                throw new PromptInfoProviderException(message);
            }
            if (promptReportParameters.Length == 1)
            {
                return _singleLevelPromptInfoProvider.GetPromptInfo(baseReportInfo, promptReportParameters[0]);
            }
            if (promptReportParameters.Length > 1 && 
                promptReportParameters.First().ValidValues == null && 
                baseReportInfo.Name.StartsWith(string.Format("{0}_",_recursiveHierarchyPrefix)))
            {
                return _recursivePromptInfoProvider.GetPromptInfo(
                    baseReportInfo,
                    promptReportParameters);
            }
            if (promptReportParameters.Length > 1 && promptReportParameters.First().ValidValues == null)
            {
                return _casscadingPromptInfoProvider.GetPromptInfo(
                    baseReportInfo,
                    promptReportParameters[0],
                    promptReportParameters[1]);
            }
            if (promptReportParameters.Length > 1 && promptReportParameters.First().ValidValues != null)
            {
                return _hierarchyPromptInfoProvider.GetPromptInfo(baseReportInfo, promptReportParameters);
            }
            
            throw new Exception();
        }
    }
}