using Moq;
using NUnit.Framework;
using Prompts.Service.PromptService;
using Test.Prompts.Service.Infastructure;

namespace Test.Prompts.Service
{
    [TestFixture]
    public class ChildPromptLevelServiceTest
    {
        private Mock<IHierarchyPromptService> _hierarchyPromptService;
        private ChildPromptLevelService _service;

        [SetUp]
        public void Setup()
        {
            _hierarchyPromptService = new Mock<IHierarchyPromptService>();
            _service = new ChildPromptLevelService(_hierarchyPromptService.Object);
        }

        [Test]
        public void ItUsesThePromptNameAndParameterValuesToGetTheHierarchyAndTheUsesTheParameterNameToGetTheChildLevel()
        {
            var request = new ChildPromptItemsRequest
                {
                    ParameterName = "Parameter Name",
                    ParameterValues = A.Array(A.ParameterValue().Build(), A.ParameterValue().Build()),
                    PromptName = "PromptName"
                };

            var hierarchyPromptLevel = A.PromptLevel().Build();

            var hierarchy = Mock.Of<IHierarchyPrompt>(
                p => p.GetChildOf(request.ParameterName) == hierarchyPromptLevel);

            _hierarchyPromptService
                .Setup(s => s.GetHierarchyPrompt(request.PromptName, request.ParameterValues))
                .Returns(hierarchy);

            var response = (PromptLevel)_service.OnPost(request);

            Assert.AreEqual(hierarchyPromptLevel, response);
        }
    }
}
