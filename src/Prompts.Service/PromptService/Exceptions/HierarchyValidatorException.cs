using System;

namespace Prompts.Service.PromptService.Exceptions
{
    public class HierarchyValidatorException : Exception
    {
        public HierarchyValidatorException(string message)
            : base(message)
        {
            
        }
    }
}