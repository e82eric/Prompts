using Moq;
using Prompts.Prompting.ViewModels;

namespace Test.Prompts.Infrastructure.Builders
{
    public class PromptBuilder
    {
        private readonly Mock<IPrompt> _mock;

        public PromptBuilder()
        {
            _mock = new Mock<IPrompt>();
        }

        public IPrompt Build()
        {
            return _mock.Object;
        }
    }
}
