using System.Net;
using System.Web.Services.Protocols;
using System.Xml;
using Moq;
using NUnit.Framework;
using Prompts.Service.PromptService;
using Prompts.Service.PromptService.Exceptions;
using Prompts.Service.PromptService.Implementation;
using Prompts.Service.ReportExecution;
using Test.Prompts.Service.Infastructure;

namespace Test.Prompts.Service
{
    [TestFixture]
    public class BaseReportParameterServiceTest
    {
        private Mock<IReportExecutionService> _reportExecutionService;
        private BaseReportParameterService _parameterService;

        [SetUp]
        public void Setup()
        {
            _reportExecutionService = new Mock<IReportExecutionService>();
            _parameterService = new BaseReportParameterService(_reportExecutionService.Object);
        }

        [Test]
        public void ItGetsTheParametersFromTheReportExecutionServiceUsingThePath()
        {
            const string path = "Path";

            var serviceParameters = A.Array(A.ReportParameter().Build(), A.ReportParameter().Build());
            var exectuionInfo = new ExecutionInfo2 {Parameters = serviceParameters};

            _reportExecutionService.Setup(s => s.LoadReport2(path, null)).Returns(exectuionInfo);

            var parameters = _parameterService.GetParametersFor(path);

            Assert.AreEqual(serviceParameters, parameters);
        }

        [Test]
        public void ItThrowsAnExectionWhenTheReportExecutionServiceThrowsASoapException()
        {
            const string path = "Path";
            const string messageInnerText = "Message Inner Text";

            var soapException = CreateSoapException(messageInnerText);

            _reportExecutionService.Setup(s => s.LoadReport2(path, null)).Throws(soapException);

            var expectedExceptionMessage = string.Format(
                "An error was returned by the Reporting Services web service: '{0}'"
                , messageInnerText);
            
            ExceptionAssert.Throws<ReportingServicesException>(expectedExceptionMessage
                , () => _parameterService.GetParametersFor(path));
        }

        [Test]
        public void ItThrowsAnExceptionWhenTheReportExecutionServiceThrowsAWebException()
        {
            const string path = "Path";
            const string message = "Message";

            var exception = new WebException(message);

            _reportExecutionService.Setup(s => s.LoadReport2(path, null)).Throws(exception);

            var expectedExceptionMessage = string.Format(
                "An error was returned by the Reporting Services web service: '{0}'"
                , message);

            ExceptionAssert.Throws<ReportingServicesException>(expectedExceptionMessage
                , () => _parameterService.GetParametersFor(path));
        }

        private static SoapException CreateSoapException(string messageInnerText)
        {
            var xmlElement = new XmlDocument();
            var detailNode = xmlElement.CreateElement("detail");
            var messageNode = xmlElement.CreateNode(XmlNodeType.Element, "Message", string.Empty);
            detailNode.AppendChild(messageNode);
            messageNode.InnerText = messageInnerText;

            return new SoapException("message", null, string.Empty, detailNode);
        }
    }
}
