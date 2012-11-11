using Moq;
using NUnit.Framework;
using Prompts.Service.PromptService;
using Prompts.Service.PromptService.Implementation;
using Prompts.Service.ReportExecution;
using Test.Prompts.Service.Infastructure;

namespace Test.Prompts.Service
{
    [TestFixture]
    public class CascadingSearchDefaultValueProviderTest
    {
        private Mock<IPromptReportParameterService> _promptReportParameterService;
        private CascadingSearchDefaultValueProvider _provider;
        private Mock<IDefaultValueProvider> _defaultValueProvider;

        [SetUp]
        public void Setup()
        {
            _defaultValueProvider = new Mock<IDefaultValueProvider>();
            _promptReportParameterService = new Mock<IPromptReportParameterService>();
            _provider = new CascadingSearchDefaultValueProvider(
                _promptReportParameterService.Object,
                _defaultValueProvider.Object);
        }

        [Test]
        public void ItReturnsTheVallidValueThatHasTheSameValueAsTheValueParameterDefaultAndTheSameLabelAsTheLabelParameter()
        {
            const string promptReportName = "PromptReportName";
            const string searchParameterName = "SearchParameterName";
            const string valueParmaeterDefault = "Value Default";
            const string labelParameterDefault = "Label Default";

            var validValueWithSameValue = A.ValidValue()
                .WithValue(valueParmaeterDefault)
                .WithLabel("Label")
                .Build();

            var validValueWithSameLabel = A.ValidValue()
                .WithValue("Value")
                .WithLabel(labelParameterDefault)
                .Build();

            const string defaultString = "String1";
            var expected = A.DefaultValue(valueParmaeterDefault);

            _defaultValueProvider
                .Setup(p => p.Get(defaultString, labelParameterDefault))
                .Returns(expected);

            var searchParameter = A.ReportParameter().Build();
            var resultParameter = A.ReportParameter()
                .WithValidValues(
                    A.Array(
                        validValueWithSameValue,
                        validValueWithSameLabel,
                        A.ValidValue().WithValue(valueParmaeterDefault).WithLabel(labelParameterDefault).Build(),
                        A.ValidValue().WithValue("Value 3").Build()))
                .Build();

            var parameters = A.Array(searchParameter, resultParameter);

            _promptReportParameterService.Setup(
                s => s.GetParametersFor(promptReportName, It.Is<ParameterValue>(
                    v =>
                    v.Name == searchParameterName &&
                    v.Value == labelParameterDefault)))
                .Returns(parameters);

            var defaultValues = _provider.Get(
                promptReportName,
                searchParameterName,
                A.Array(defaultString),
                A.Array(labelParameterDefault));

            defaultValues.AssetItemsAndLength(expected);
        }

        [Test]
        public void ItUsesTheFirstDefaultWhenThereAreMoreThanOneDefault()
        {
            const string promptReportName = "PromptReportName";
            const string searchParameterName = "SearchParameterName";
            const string valueParmaeterDefault1 = "Value Default 1";
            const string valueParameterDefault2 = "Value Default 2";
            const string labelParameterDefault1 = "Label Default 1";
            const string labelParmaeterDefault2 = "Label Default 2";

            var validValueWithSameValue = A.ValidValue()
                .WithValue(valueParmaeterDefault1)
                .WithLabel("Label")
                .Build();

            var validValueWithSameLabel = A.ValidValue()
                .WithValue("Value")
                .WithLabel(labelParameterDefault1)
                .Build();

            const string defaultString1 = "String1";
            const string defaultString2 = "String2";
            var default1 = A.DefaultValue().WithValue(valueParmaeterDefault1).WithLabel(labelParameterDefault1).Build();
            var default2 = A.DefaultValue().WithValue(valueParameterDefault2).WithLabel(labelParmaeterDefault2).Build();
            _defaultValueProvider.Setup(p => p.Get(defaultString1, labelParameterDefault1)).Returns(default1);
            _defaultValueProvider.Setup(p => p.Get(defaultString2, labelParmaeterDefault2)).Returns(default2);

            var searchParameter = A.ReportParameter().Build();
            var resultParameter = A.ReportParameter()
                .WithValidValues(
                    A.Array(
                        validValueWithSameValue,
                        validValueWithSameLabel,
                        A.ValidValue().WithValue(valueParmaeterDefault1).WithLabel(labelParameterDefault1).Build(),
                        A.ValidValue().WithValue("Value 3").Build()))
                .Build();

            var parameters = A.Array(searchParameter, resultParameter);

            _promptReportParameterService.Setup(
                s => s.GetParametersFor(promptReportName, It.Is<ParameterValue>(
                    v =>
                    v.Name == searchParameterName &&
                    v.Value == labelParameterDefault1)))
                .Returns(parameters);

            var defaultValues = _provider.Get(
                promptReportName,
                searchParameterName,
                A.Array(defaultString1, defaultString2),
                A.Array(labelParameterDefault1, labelParmaeterDefault2));

            defaultValues.AssetItemsAndLength(default1);
        }

        [Test]
        public void ItItReturnsAnEmptyCollectionWhenTheLabelMatchesButTheValueDoesNot()
        {
            const string promptReportName = "PromptReportName";
            const string searchParameterName = "SearchParameterName";
            const string valueParmaeterDefault1 = "Value Default 1";
            const string labelParameterDefault1 = "Label Default 1";

            const string defaultString = "String1";
            var defaultValue = A.DefaultValue(valueParmaeterDefault1);
            _defaultValueProvider.Setup(p => p.Get(defaultString, labelParameterDefault1)).Returns(defaultValue);

            var valueWithSameLabel = A.ValidValue()
                .WithValue("Value")
                .WithLabel(labelParameterDefault1)
                .Build();

            var searchParameter = A.ReportParameter().Build();
            var resultParameter = A.ReportParameter()
                .WithValidValues(
                    A.Array(
                        valueWithSameLabel,
                        A.ValidValue().WithValue("Value 3").Build(),
                        A.ValidValue().WithValue("Value 3").Build()))
                .Build();

            var parameters = A.Array(searchParameter, resultParameter);

            _promptReportParameterService.Setup(
                s => s.GetParametersFor(promptReportName, It.Is<ParameterValue>(
                    v =>
                    v.Name == searchParameterName &&
                    v.Value == labelParameterDefault1)))
                .Returns(parameters);

            var defaultValues = _provider.Get(
                promptReportName,
                searchParameterName,
                A.Array(defaultString),
                A.Array(labelParameterDefault1));

            defaultValues.AssertLength(0);
        }

        [Test]
        public void ItItReturnsAnEmptyCollectionWhenTheValueMatchesButTheLabelDoesNot()
        {
            const string promptReportName = "PromptReportName";
            const string searchParameterName = "SearchParameterName";
            const string valueParmaeterDefault1 = "Value Default 1";
            const string labelParameterDefault1 = "Label Default 1";

            const string defaultString = "String1";
            var defaultValue = A.DefaultValue(valueParmaeterDefault1);
            _defaultValueProvider.Setup(p => p.Get(defaultString, labelParameterDefault1)).Returns(defaultValue);

            var valueWithSameLabel = A.ValidValue()
                .WithValue(valueParmaeterDefault1)
                .WithLabel("Label")
                .Build();

            var searchParameter = A.ReportParameter().Build();
            var resultParameter = A.ReportParameter()
                .WithValidValues(
                    A.Array(
                        valueWithSameLabel,
                        A.ValidValue().WithValue("Value 2").Build(),
                        A.ValidValue().WithValue("Value 3").Build()))
                .Build();

            var parameters = A.Array(searchParameter, resultParameter);

            _promptReportParameterService.Setup(
                s => s.GetParametersFor(promptReportName, It.Is<ParameterValue>(
                    v =>
                    v.Name == searchParameterName &&
                    v.Value == labelParameterDefault1)))
                .Returns(parameters);

            var defaultValues = _provider.Get(
                promptReportName,
                searchParameterName,
                A.Array(defaultString),
                A.Array(labelParameterDefault1));

            defaultValues.AssertLength(0);
        }


        [Test]
        public void ItReturnsTheFirstVallidValueThatHasTheSameValueAsTheValueParameterDefaultAndTheSameLabelAsTheLabelParameter()
        {
            const string promptReportName = "PromptReportName";
            const string searchParameterName = "SearchParameterName";
            const string valueParmaeterDefault = "Value Default";
            const string labelParameterDefault = "Label Default";

            var matchingValidValue2 = A.ValidValue()
                .WithValue(valueParmaeterDefault)
                .WithLabel(labelParameterDefault)
                .Build();

            const string defaultString = "String1";
            var defaultValue = A.DefaultValue(valueParmaeterDefault);
            _defaultValueProvider.Setup(p => p.Get(defaultString, labelParameterDefault)).Returns(defaultValue);

            var searchParameter = A.ReportParameter().Build();
            var resultParameter = A.ReportParameter()
                .WithValidValues(
                    A.Array(
                        A.ValidValue().WithValue("Value 1").Build(),
                        A.ValidValue().WithValue(valueParmaeterDefault).WithLabel(labelParameterDefault).Build(),
                        matchingValidValue2,
                        A.ValidValue().WithValue("Value 3").Build()))
                .Build();

            var parameters = A.Array(searchParameter, resultParameter);

            _promptReportParameterService.Setup(
                s => s.GetParametersFor(promptReportName, It.Is<ParameterValue>(
                    v =>
                    v.Name == searchParameterName &&
                    v.Value == labelParameterDefault)))
                .Returns(parameters);

            var defaultValues = _provider.Get(
                promptReportName,
                searchParameterName,
                A.Array(defaultString),
                A.Array(labelParameterDefault));

            defaultValues.AssetItemsAndLength(defaultValue);
        }

        [Test]
        public void ItReturnsAEmptyCollectionAndDoesNotCallThePromptServiceWhenThereAreNotDefaults()
        {
            const string promptReportName = "PromptReportName";
            const string searchParameterName = "SearchParameterName";

            var defaultValues = _provider.Get(
                promptReportName,
                searchParameterName,
                new string[]{},
                new string[]{});

            _promptReportParameterService.Verify(
                s => s.GetParametersFor(
                    It.IsAny<string>(),
                    It.IsAny<ParameterValue>())
                , Times.Exactly(0));

            defaultValues.AssertLength(0);
        }

        [Test]
        public void ItReturnsAEmptyCollectionAndDoesNotCallThePromptServiceWhenThereIsALabelParameterDefaultButNoValueParameterDefault()
        {
            const string promptReportName = "PromptReportName";
            const string searchParameterName = "SearchParameterName";

            var defaultValues = _provider.Get(
                promptReportName,
                searchParameterName,
                new string[] { },
                A.Array("Label Default"));

            _promptReportParameterService.Verify(
                s => s.GetParametersFor(
                    It.IsAny<string>(),
                    It.IsAny<ParameterValue>())
                , Times.Exactly(0));

            defaultValues.AssertLength(0);
        }

        [Test]
        public void ItReturnsAEmptyCollectionAndDoesNotCallThePromptServiceWhenThereIsAValueParameterDefaultButNoLabelParameterDefault()
        {
            const string promptReportName = "PromptReportName";
            const string searchParameterName = "SearchParameterName";

            var defaultValues = _provider.Get(
                promptReportName,
                searchParameterName,
                A.Array("Value Default"),
                new string[] { });

            _promptReportParameterService.Verify(
                s => s.GetParametersFor(
                    It.IsAny<string>(),
                    It.IsAny<ParameterValue>())
                , Times.Exactly(0));

            defaultValues.AssertLength(0);
        }
    }
}
