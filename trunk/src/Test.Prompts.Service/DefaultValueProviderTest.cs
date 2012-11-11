using NUnit.Framework;
using Prompts.Service.PromptService.Implementation;

namespace Test.Prompts.Service
{
    [TestFixture]
    public class DefaultValueProviderTest
    {
        [Test]
        public void ItUsesTheValueForTheValue()
        {
            const string value = "Some Value";

            var provider = new DefaultValueProvider("AllMemberPrefix");

            var defaultValue = provider.Get(value, "Some Label");

            Assert.AreEqual(value, defaultValue.Value);
        }

        [Test]
        public void ItUsesTheLabelForTheLabel()
        {
            const string label = "Some Label";

            var provider = new DefaultValueProvider("AllMemberPrefix");
            
            var defaultValue = provider.Get("Some Value", label);

            Assert.AreEqual(label, defaultValue.Label);
        }

        [Test]
        public void ItSetsTheIsAllMemberToFalseWhenTheValueDoesNotStartWithThePrefix()
        {
            var provider = new DefaultValueProvider("AllMemberPrefix");

            var defaultValue = provider.Get("Some Value", "Some Label");

            Assert.IsFalse(defaultValue.IsAllMember);
        }

        [Test]
        public void ItSetsTheIsAllMemberToTrueWhenTheValueDoesStartWithThePrefix()
        {
            const string allMemberPrefix = "AllMemberPrefix";
            var provider = new DefaultValueProvider(allMemberPrefix);

            var defaultValue = provider.Get(string.Format("{0}SomeValue", allMemberPrefix), "Some Label");

            Assert.IsTrue(defaultValue.IsAllMember);
        }

        [Test]
        public void ItSetsTheIsAllMemberToFalseWhenTheValueEndsWithThePrefix()
        {
            const string allMemberPrefix = "AllMemberPrefix";
            var provider = new DefaultValueProvider(allMemberPrefix);

            var defaultValue = provider.Get(string.Format("SomeValue{0}", allMemberPrefix), "Some Label");

            Assert.IsFalse(defaultValue.IsAllMember);
        }

        [Test]
        public void ItSetsTheIsAllMemberToFalseWhenTheValueContainsWithThePrefix()
        {
            const string allMemberPrefix = "AllMemberPrefix";
            var provider = new DefaultValueProvider(allMemberPrefix);

            var defaultValue = provider.Get(string.Format("S{0}omeValue", allMemberPrefix), "Some Label");

            Assert.IsFalse(defaultValue.IsAllMember);
        }

        [Test]
        public void ItRemovesThePrefixFromTheValueStartsWithThePrefix()
        {
            const string allMemberPrefix = "AllMemberPrefix";

            const string expected = "SomeValue";

            var provider = new DefaultValueProvider(allMemberPrefix);

            var defaultValue = provider.Get(string.Format("{0}{1}", allMemberPrefix, expected), "Some Label");

            Assert.AreEqual(expected, defaultValue.Value);
        }

        [Test]
        public void ItDoesNotRemoveThePrefixFromTheValueEndsWithThePrefix()
        {
            const string allMemberPrefix = "AllMemberPrefix";

            var provider = new DefaultValueProvider(allMemberPrefix);

            var expected = string.Format("SomeValue{0}", allMemberPrefix);

            var defaultValue = provider.Get(expected, "Some Label");

            Assert.AreEqual(expected, defaultValue.Value);
        }

        [Test]
        public void ItDoesNotRemoveThePrefixFromTheValueContainshThePrefix()
        {
            const string allMemberPrefix = "AllMemberPrefix";

            var provider = new DefaultValueProvider(allMemberPrefix);

            var expected = string.Format("SomeValu{0}e", allMemberPrefix);

            var defaultValue = provider.Get(expected, "Some Label");

            Assert.AreEqual(expected, defaultValue.Value);
        }
    }
}
