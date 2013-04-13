using System;
using System.Linq;
using Prompts.Service.PromptService.Exceptions;
using Prompts.Service.ReportExecution;

namespace Prompts.Service.PromptService.Implementation
{
    public class HierarchyPromptReportValidator : IHierarchyPromptReportValidator
    {
        public void Validate(string promptName, ReportParameter[] promptReportParameters)
        {
            for (var i = 0; i < promptReportParameters.Length - 1; i++)
            {
                var childParameter = promptReportParameters[i + 1];
                var parentParameter = promptReportParameters[i];

                if (childParameter.Dependencies == null)
                {
                    throw GetInvalidDependencyException(promptName, parentParameter.Name, childParameter.Name);
                }
                if (childParameter.Dependencies.Length != 1)
                {
                    throw GetMoreThanOneDependencyException(promptName, childParameter.Name);
                }
                if (childParameter.Dependencies.Single().Equals(parentParameter.Name) == false)
                {
                    throw GetInvalidDependencyException(promptName, parentParameter.Name, childParameter.Name);
                }
            }
        }

        private static Exception GetInvalidDependencyException(string promptName, string parentParameterName, string childParameterName)
        {
            const string exceptionMessageFormat =
                "An error occured validating the Tree Prompt '{0}', parameter '{1}' was not dependent on parameter '{2}'";

            var exceptionMessage = string.Format(
                exceptionMessageFormat
                , promptName
                , childParameterName
                , parentParameterName);

            return GetException(exceptionMessage);
        }

        private static Exception GetMoreThanOneDependencyException(string promptName, string childParameterName)
        {
            const string exceptionMessageFormat =
                "An error occured validating the Tree Prompt '{0}', parameter '{1}' has more than one dependency";

            var exceptionMessage = string.Format(
                exceptionMessageFormat
                , promptName
                , childParameterName);

            return GetException(exceptionMessage);
        }

        private static Exception GetException(string message)
        {
            return new HierarchyValidatorException(message);
        }
    }
}