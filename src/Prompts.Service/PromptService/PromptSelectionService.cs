using ServiceStack.ServiceInterface;

namespace Prompts.Service.PromptService
{
    public class PromptSelectionService : RestServiceBase<SetPromptSelectionsRequest>
    {
         private readonly IBaseReportParameterService _parameterService;
         private readonly ISelectionParameterValueBuilder _selectionParameterValueBuilder;

         public PromptSelectionService(
            IBaseReportParameterService parameterService
            , ISelectionParameterValueBuilder selectionParameterValueBuilder)
         {
             _selectionParameterValueBuilder = selectionParameterValueBuilder;
             _parameterService = parameterService;
         }

         public override object OnPost(SetPromptSelectionsRequest request)
         {
             var baseReportParameters = _parameterService.GetParametersFor(request.Path);
             var parameterValues = _selectionParameterValueBuilder.Get(baseReportParameters, request.PromptSelections);
             return _parameterService.SetParameters(parameterValues);
         }
    }
}
