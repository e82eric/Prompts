using System;

namespace Prompts.Service.PromptService.Exceptions
{
    public class PromptTypeProviderException : Exception
    {
        public PromptTypeProviderException(string message)
            :base(message)
        {
            
        }
    }
}