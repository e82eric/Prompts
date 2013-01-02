using Prompts.Service.PromptService.Exceptions;
using Prompts.Service.ReportExecution;

namespace Prompts.Service.PromptService.Implementation
{
    public class GlobalPromptBaseReportInfoMapper : IGlobalPromptBaseReportInfoMapper
    {
        public GlobalPromptBaseReportInfo Map(ReportParameter valueParameter, ReportParameter labelParameter)
        {
            SelectionType selectionType;

            if(valueParameter.MultiValue != labelParameter.MultiValue)
            {
                var expectionMessage =
                    string.Format(
                        "An error occured getting the selection type for '{0}':  both parameters did not have the same multivalue flag"
                        , valueParameter.Name);

                throw new GlobalPromptBaseReportInfoMapperException(expectionMessage);
            }

            if(valueParameter.MultiValue && labelParameter.MultiValue)
            {
                selectionType = SelectionType.MultiSelect;
            }
            else
            {
                selectionType = SelectionType.SingleSelect;
            }

            return new GlobalPromptBaseReportInfo(
                valueParameter.Name
                , valueParameter.Prompt
                , valueParameter.DefaultValues ?? new string[]{}
                , labelParameter.DefaultValues ?? new string[]{}
                , selectionType);
        }
    }
}