using System;

namespace Test.Prompts.Infrastructure
{
    public class CallbackInterceptor
    {
        private Action _action;

        public bool Intercept(Action actionToCapture)
        {
            _action = actionToCapture;
            return true;
        }

        public void ExecuteCallback()
        {
            _action();
        }
    }
}
