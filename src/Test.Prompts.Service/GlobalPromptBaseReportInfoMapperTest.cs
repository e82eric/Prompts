using NUnit.Framework;
using Prompts.Service.PromptService;
using Prompts.Service.PromptService.Exceptions;
using Prompts.Service.PromptService.Implementation;
using Test.Prompts.Service.Infastructure;

namespace Test.Prompts.Service
{
    [TestFixture]
    public class GlobalPromptBaseReportInfoMapperTest
    {
        private GlobalPromptBaseReportInfoMapper _mapper;

        [SetUp]
        public void Setup()
        {
            _mapper = new GlobalPromptBaseReportInfoMapper();
        }

        [Test]
        public void ItUsesTheValueParametersName()
        {
            var valueParameter = A.ReportParameter().WithName("Value Parameter").Build();
            var labelParmaeter = A.ReportParameter().WithName("Label Parameter").Build();

            var baseReportInfo = _mapper.Map(valueParameter, labelParmaeter);

            Assert.AreEqual(valueParameter.Name, baseReportInfo.Name);
        }

        [Test]
        public void ItUsesTheValueParametersPromptForLabel()
        {
            var valueParameter = A.ReportParameter().WithPrompt("Value Parameter").Build();
            var labelParmaeter = A.ReportParameter().WithPrompt("Label Parameter").Build();

            var baseReportInfo = _mapper.Map(valueParameter, labelParmaeter);

            Assert.AreEqual(valueParameter.Prompt, baseReportInfo.Label);
        }

        [Test]
        public void ItSetsTheValueParameterDefaults()
        {
            var valueParameter = A.ReportParameter().WithDefaultValues("Value 1", "Value 2").Build();
            var labelParmaeter = A.ReportParameter().WithDefaultValues(null).Build();

            var baseReportInfo = _mapper.Map(valueParameter, labelParmaeter);

            Assert.AreEqual(valueParameter.DefaultValues, baseReportInfo.ValueParameterDefaults);
        }

        [Test]
        public void ItSetsTheValueParameterDefaultsToAnEmptyCollectionWhenTheParameterDefaultsAreNull()
        {
            var valueParameter = A.ReportParameter().WithDefaultValues(null).Build();
            var labelParmaeter = A.ReportParameter().WithDefaultValues(null).Build();

            var baseReportInfo = _mapper.Map(valueParameter, labelParmaeter);

            baseReportInfo.ValueParameterDefaults.AssertLength(0);
        }

        [Test]
        public void ItSetsTheLabelParameterDefaults()
        {
            var valueParameter = A.ReportParameter().WithDefaultValues(null).Build();
            var labelParmaeter = A.ReportParameter().WithDefaultValues("Value 1", "Value 2").Build();

            var baseReportInfo = _mapper.Map(valueParameter, labelParmaeter);

            Assert.AreEqual(labelParmaeter.DefaultValues, baseReportInfo.LabelParameterDefaults);
        }

        [Test]
        public void ItSetsTheLabelParameterDefaultsToAnEmptyCollectionWhenTheParametersDefaultsAreNull()
        {
            var valueParameter = A.ReportParameter().WithDefaultValues(null).Build();
            var labelParmaeter = A.ReportParameter().WithDefaultValues(null).Build();

            var baseReportInfo = _mapper.Map(valueParameter, labelParmaeter);

            baseReportInfo.LabelParameterDefaults.AssertLength(0);
        }

        [Test]
        public void ItSetsTheSelectionTypeToSingleWhenBothParameterAreNotMultiValue()
        {
            var valueParameter = A.ReportParameter().WithMultiValueFlag(false).Build();
            var labelParmaeter = A.ReportParameter().WithMultiValueFlag(false).Build();

            var baseReportInfo = _mapper.Map(valueParameter, labelParmaeter);

            Assert.AreEqual(SelectionType.SingleSelect, baseReportInfo.SelectionType);
        }

        [Test]
        public void ItSetsTheSelectionTypeToMultiSelectWhenBothParameterAreMultiValue()
        {
            var valueParameter = A.ReportParameter().WithMultiValueFlag(true).Build();
            var labelParmaeter = A.ReportParameter().WithMultiValueFlag(true).Build();

            var baseReportInfo = _mapper.Map(valueParameter, labelParmaeter);

            Assert.AreEqual(SelectionType.MultiSelect, baseReportInfo.SelectionType);
        }

        [Test]
        public void ItThrowsAnExceptionWhenTheValueParameterIsMultiValueAndTheLabelParameterIsNot()
        {
            var valueParameter = A.ReportParameter().WithMultiValueFlag(true).Build();
            var labelParmaeter = A.ReportParameter().WithMultiValueFlag(false).Build();

            var expectedExceptionMessage =
                string.Format(
                    "An error occured getting the selection type for '{0}':  both parameters did not have the same multivalue flag"
                    , valueParameter.Name);

            ExceptionAssert.Throws<GlobalPromptBaseReportInfoMapperException>(expectedExceptionMessage
                , () => _mapper.Map(valueParameter, labelParmaeter));
        }

        [Test]
        public void ItThrowsAnExceptionWhenTheValueParameterIsNotMultiValueAndTheLabelParameterIs()
        {
            var valueParameter = A.ReportParameter().WithMultiValueFlag(false).Build();
            var labelParmaeter = A.ReportParameter().WithMultiValueFlag(true).Build();

            var expectedExceptionMessage =
                string.Format(
                    "An error occured getting the selection type for '{0}':  both parameters did not have the same multivalue flag"
                    , valueParameter.Name);

            ExceptionAssert.Throws<GlobalPromptBaseReportInfoMapperException>(expectedExceptionMessage
                , () => _mapper.Map(valueParameter, labelParmaeter));
        }
    }
}
