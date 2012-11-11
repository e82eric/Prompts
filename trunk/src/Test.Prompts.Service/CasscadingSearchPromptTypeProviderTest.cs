using System;
using NUnit.Framework;
using Prompts.Service.PromptService;
using Prompts.Service.PromptService.Exceptions;
using Prompts.Service.PromptService.Implementation;
using Test.Prompts.Service.Infastructure;

namespace Test.Prompts.Service
{
    [TestFixture]
    public class CasscadingSearchPromptTypeProviderTest
    {
        private CasscadingSearchPromptTypeProvider _provider;

        [SetUp]
        public void Setup()
        {
            _provider = new CasscadingSearchPromptTypeProvider();
        }

        [Test]
        public void ItReturnsCasscadingSearchWhenMultiSelect()
        {
            const SelectionType selectionType = SelectionType.MultiSelect;

            var promptType = _provider.GetPromptType(selectionType);

            Assert.AreEqual(PromptType.CasscadingSearch, promptType);
        }

        [Test]
        public void ItThrowsAnExceptionWhenTheSingleSelect()
        {
            const SelectionType selectionType = SelectionType.SingleSelect;

            ExceptionAssert.Throws<PromptTypeProviderException>(
                "A casscading search prompt must be multi-select",
                () => _provider.GetPromptType(selectionType));
        }
    }
}
