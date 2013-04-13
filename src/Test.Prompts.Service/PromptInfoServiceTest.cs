using Moq;
using NUnit.Framework;
using Prompts.Service.PromptService;
using Prompts.Service.PromptService.Implementation;
using Test.Prompts.Service.Infastructure;

namespace Test.Prompts.Service
{
    [TestFixture]
    public class PromptInfoServiceTest
    {
        [Test]
        public void ItCorrectlyCordinatesTheBaseReportParameterServiceAndInterpreter()
        {
            const string path = "Report Path";

            var parametersForServiceToReturn = A.Array(A.ReportParameter().Build());

            var parameterService = Mock.Of<IBaseReportParameterService>(
                s => s.GetParametersFor(path) == parametersForServiceToReturn);

            var promptsForInterpreterToReturn = A.Array(A.PromptInfo().Build());

            var interpreter = Mock.Of<IBaseReportInterpreter<PromptInfo>>(
                i => i.Get(parametersForServiceToReturn) == promptsForInterpreterToReturn);

            var promptInfoService = new PromptInfoService(parameterService, interpreter);

            var servicePrompts = promptInfoService.GetPrompts(path);

            Assert.AreEqual(promptsForInterpreterToReturn, servicePrompts);
        }
    }
}
