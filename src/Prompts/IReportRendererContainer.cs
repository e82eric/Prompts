using Prompts.ReportRendering.ViewModel;

namespace Prompts
{
    public interface IReportRendererContainer
    {
        IReportRenderer Create();
    }
}