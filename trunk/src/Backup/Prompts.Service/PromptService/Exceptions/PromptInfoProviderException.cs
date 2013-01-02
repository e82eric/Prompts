using System;

namespace Prompts.Service.PromptService.Exceptions
{
    public class PromptInfoProviderException : Exception
    {
        public PromptInfoProviderException(string message) : base(message)
        {
        }
    }
}