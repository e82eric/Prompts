using NUnit.Framework;
using Prompts.Service.PromptService.Implementation;
using Test.Prompts.Service.Infastructure;

namespace Test.Prompts.Service
{
    [TestFixture]
    public class HierarchyPromptTest
    {
        [Test]
        public void ItUsesTheNameAndValidValuesOfTheParametersThatsDepencyEqualsTheParametersName()
        {
            var parameter1 = A.ReportParameter().WithName("Parameter 1").Build();
            var parameter2 = A.ReportParameter().WithName("Parmaeter 2").WithDependencies(parameter1.Name).Build();
            var parameter3 = A.ReportParameter().WithName("Parmaeter 3").WithDependencies(parameter2.Name).Build();
            var parameters = A.Array(parameter1, parameter2, parameter3);

            var prompt = new HierarchyPrompt(parameters);

            var childPromptLevel = prompt.GetChildOf(parameter1.Name);

            Assert.AreEqual(parameter2.Name, childPromptLevel.ParameterName);
            Assert.AreEqual(parameter2.ValidValues, childPromptLevel.AvailableItems);
        }

        [Test]
        public void HasChildIsTrueWhenAnotherParameterIsDependentOnTheChildParameter()
        {
            var parameter1 = A.ReportParameter().WithName("Parameter 1").Build();
            var parameter2 = A.ReportParameter().WithName("Parmaeter 2").WithDependency(parameter1.Name).Build();
            var parameter3 = A.ReportParameter().WithName("Parmaeter 3").WithDependency(parameter2.Name).Build();
            var parameters = new[] { parameter1, parameter2, parameter3 };

            var prompt = new HierarchyPrompt(parameters);

            var childPromptLevel = prompt.GetChildOf(parameter1.Name);

            Assert.IsTrue(childPromptLevel.HasChildLevel);
        }

        [Test]
        public void HasChildIsFalseWhenNoParametersAreDependentOnTheChildParameter()
        {
            var parameter1 = A.ReportParameter().WithName("Parameter 1").Build();
            var parameter2 = A.ReportParameter().WithName("Parmaeter 2").WithDependencies(parameter1.Name).Build();
            var parameter3 = A.ReportParameter().WithName("Parmaeter 3").WithDependencies(parameter2.Name).Build();
            var parameters = A.Array(parameter1, parameter2, parameter3);

            var prompt = new HierarchyPrompt(parameters);

            var childPromptLevel = prompt.GetChildOf(parameter2.Name);

            Assert.IsFalse(childPromptLevel.HasChildLevel);
        }

        [Test]
        public void ItSetsTheAvailableItemsToAnEmptyCollectionWhenTheValidValuesAreNull()
        {
            var parameter1 = A.ReportParameter().WithName("Parameter 1").Build();
            var parameter2 = A.ReportParameter().WithName("Parmaeter 2").WithDependencies(parameter1.Name).Build();
            var parameter3 = A.ReportParameter().WithName("Parmaeter 3").WithDependencies(parameter2.Name).Build();
            var parameters = A.Array(parameter1, parameter2, parameter3);

            var prompt = new HierarchyPrompt(parameters);

            var childPromptLevel = prompt.GetChildOf(parameter1.Name);

            Assert.AreEqual(parameter2.Name, childPromptLevel.ParameterName);
            Assert.AreEqual(parameter2.ValidValues, childPromptLevel.AvailableItems);
        }
    }
}
