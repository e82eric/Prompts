using System.Collections.Generic;
using Prompts.Service.ReportExecution;

namespace Prompts.Service.PromptService.Implementation
{
    public class SelectionParameterValueBuilder : ISelectionParameterValueBuilder
    {
        private readonly IBaseReportInterpreter<IParameterValueBuilder> _baseReportInterpreter;
        private readonly IPromptSelectionsProvider _promptSelectionsProvider;

        public SelectionParameterValueBuilder(
            IBaseReportInterpreter<IParameterValueBuilder> baseReportInterpreter,
            IPromptSelectionsProvider promptSelectionsProvider)
        {
            _promptSelectionsProvider = promptSelectionsProvider;
            _baseReportInterpreter = baseReportInterpreter;
        }

        public ParameterValue[] Get(ReportParameter[] baseReportParmaeters, IPromptSelections promptSelections)
        {
            var parameterValuesToReturn = new List<ParameterValue>();

            var baseReportPrompts = _baseReportInterpreter.Get(baseReportParmaeters);

            foreach (var parameterValueBuilder in baseReportPrompts)
            {
                var promptParmaeterValues = promptSelections.CreateParameterValuesFor(parameterValueBuilder);
                parameterValuesToReturn.AddRange(promptParmaeterValues);
            }

            return parameterValuesToReturn.ToArray();
        }

        public ParameterValue[] Get(ReportParameter[] baseReportParameters, IEnumerable<PromptSelectionInfo> promptSelectionInfos)
        {
            var promptSelections = _promptSelectionsProvider.Get(promptSelectionInfos);

            var parameterValuesToReturn = new List<ParameterValue>();

            var baseReportPrompts = _baseReportInterpreter.Get(baseReportParameters);

            foreach (var parameterValueBuilder in baseReportPrompts)
            {
                var promptParmaeterValues = promptSelections.CreateParameterValuesFor(parameterValueBuilder);
                parameterValuesToReturn.AddRange(promptParmaeterValues);
            }

            return parameterValuesToReturn.ToArray();
        }
    }
}